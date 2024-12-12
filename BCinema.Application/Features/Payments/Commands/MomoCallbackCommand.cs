using System.Globalization;
using BCinema.Application.Exceptions;
using BCinema.Application.Mail;
using BCinema.Domain.Entities;
using BCinema.Domain.Interfaces.IRepositories;
using MediatR;

namespace BCinema.Application.Features.Payments.Commands;

public class MomoCallbackCommand : IRequest<string>
{
    public string OrderId { get; set; } = default!;
    public string ErrorCode { get; set; } = default!;
    
    public class MomoCallbackCommandHandler(
        IPaymentRepository paymentRepository,
        IMailService mailService,
        IPaymentDetailRepository paymentDetailRepository,
        IScheduleRepository scheduleRepository,
        IRoomRepository roomRepository,
        ISeatScheduleRepository seatScheduleRepository,
        ISeatRepository seatRepository,
        IUserRepository userRepository,
        IFoodRepository foodRepository) : IRequestHandler<MomoCallbackCommand, string>
    {
        public async Task<string> Handle(MomoCallbackCommand request, CancellationToken cancellationToken)
        {
            var payment = await paymentRepository.GetPaymentByIdAsync(Guid.Parse(request.OrderId), cancellationToken)
                          ?? throw new NotFoundException(nameof(Payment));
            
            var firstPaymentDetail = payment.PaymentDetails.FirstOrDefault();
            
            var seatSchedule = await seatScheduleRepository
                               .GetSeatScheduleByIdAsync((Guid)firstPaymentDetail!.SeatScheduleId!, cancellationToken) 
                               ?? throw new NotFoundException(nameof(SeatSchedule));
            
            var user = await userRepository.GetByIdAsync(payment.UserId, cancellationToken)
                       ?? throw new NotFoundException(nameof(User));
            
            if (request.ErrorCode != "0")
            {
                seatSchedule.Status = SeatSchedule.SeatScheduleStatus.Available;
                await seatScheduleRepository.SaveChangesAsync(cancellationToken);
            
                user.Point -= CalculateTotalPoint(payment);
            
                user.Point += CalculatePointUsed(payment);
                
                foreach (var item in payment.PaymentDetails)
                {
                    if (item.FoodId is null) continue;
                    
                    var food = await foodRepository.GetFoodByIdAsync(item.FoodId.Value, cancellationToken)
                               ?? throw new NotFoundException(nameof(Food));
                        
                    food.Quantity += item.FoodQuantity ?? 0;
                    await foodRepository.SaveChangesAsync(cancellationToken);
                }
                
                await paymentDetailRepository.DeletePaymentDetails(payment.PaymentDetails, cancellationToken);
                await paymentRepository.DeletePaymentAsync(payment, cancellationToken);
                
                return request.ErrorCode;
            }
            
            seatSchedule.Status = SeatSchedule.SeatScheduleStatus.Bought;
            
            await seatScheduleRepository.SaveChangesAsync(cancellationToken);
            await userRepository.SaveChangesAsync(cancellationToken);
            
            await SendMailTicket(user.Email, payment, seatSchedule, cancellationToken);

            return request.ErrorCode;
        }

        private static int CalculateTotalPoint(Payment payment)
        {
            return (int)payment.TotalPrice / 10;
        }

        private static int CalculatePointUsed(Payment payment)
        {
            var voucher = payment.Voucher ?? null;

            var totalPrice = voucher != null
                ? payment.TotalPrice - payment.TotalPrice * (voucher.Discount / 100.0)
                : payment.TotalPrice;

            totalPrice -= payment.PaymentDetails.Sum(pd => pd.Price);

            return (int)(totalPrice * 100) / 100000;
        }

        private async Task SendMailTicket(string email, Payment payment, SeatSchedule seatSchedule,
            CancellationToken cancellationToken)
        {
            var schedule = scheduleRepository.GetScheduleByIdAsync(seatSchedule.ScheduleId, cancellationToken)
                           ?? throw new NotFoundException(nameof(Schedule));
            
            var room = roomRepository.GetRoomByIdAsync(schedule.Result!.RoomId, cancellationToken)
                       ?? throw new NotFoundException(nameof(Room));
            
            var roomName = room.Result!.Name;
            var movieName = schedule.Result!.MovieName;
            var movieDate = schedule.Result!.Date;
            var totalPrices = payment.TotalPrice;
            List<string> seatNumbers = [];
            
            foreach (var item in payment.PaymentDetails)
            {
                if (item.SeatScheduleId is null) continue;
                var seatS = await seatScheduleRepository
                    .GetSeatScheduleByIdAsync((Guid)item.SeatScheduleId, cancellationToken)
                            ?? throw new NotFoundException(nameof(SeatSchedule));
                
                var seat = await seatRepository.GetSeatByIdAsync(seatS.SeatId, cancellationToken)
                           ?? throw new NotFoundException(nameof(Seat));
                
                seatNumbers.Add(seat.Row + seat.Number);
            }
            
            var seatNumbersString = string.Join(", ", seatNumbers);

            var templatePath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "MailTemplate",
                "TicketTemplate.html"
            );
            
            var htmlTemplate = await File.ReadAllTextAsync(templatePath, cancellationToken);
            
            var emailBody = htmlTemplate
                .Replace("{MovieTitle}", movieName)
                .Replace("{MovieDate}", movieDate.ToString("dd/MM/yyyy"))
                .Replace("{MovieTime}", movieDate.ToString("HH:mm"))
                .Replace("{CinemaName}", "BCinema")
                .Replace("{RoomNumber}", roomName)
                .Replace("{SeatNumbers}", seatNumbersString)
                .Replace("{TotalPrice}", totalPrices.ToString(CultureInfo.InvariantCulture));
            
            var mailData = new MailData()
            {
                EmailToId = email,
                EmailSubject = "BCinema Ticket",
                EmailBody = emailBody
            };

            await mailService.SendMailAsync(mailData, cancellationToken);
        }
    }
}