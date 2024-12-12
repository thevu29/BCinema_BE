using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Application.Exceptions;
using BCinema.Domain.Entities;
using BCinema.Domain.Interfaces.IRepositories;
using MediatR;

namespace BCinema.Application.Features.Payments.Commands;

public class CreatePaymentCommand : IRequest<PaymentDto>
{
    public Guid UserId { get; set; }
    public Guid ScheduleId { get; set; }
    public Guid? VoucherId { get; set; }
    public int? Point { get; set; }
    public IEnumerable<PaymentDetailDto> PaymentDetails { get; set; } = default!;

    public class CreatePaymentCommandHandler(
        IPaymentRepository paymentRepository,
        IPaymentDetailRepository paymentDetailRepository,
        IUserRepository userRepository,
        IScheduleRepository scheduleRepository,
        IVoucherRepository voucherRepository,
        IUserVoucherRepository userVoucherRepository,
        IFoodRepository foodRepository,
        ISeatRepository seatRepository,
        ISeatScheduleRepository seatScheduleRepository,
        IMapper mapper) : IRequestHandler<CreatePaymentCommand, PaymentDto>
    {
        public async Task<PaymentDto> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetByIdAsync(request.UserId, cancellationToken)
                ?? throw new NotFoundException(nameof(User));

            var schedule = await scheduleRepository.GetScheduleByIdAsync(request.ScheduleId, cancellationToken)
                ?? throw new NotFoundException(nameof(Schedule));

            switch (schedule.Status)
            {
                case Schedule.ScheduleStatus.Ended:
                    throw new BadRequestException("Schedule is already finished");
                case Schedule.ScheduleStatus.Cancelled:
                    throw new BadRequestException("Schedule is already cancelled");
            }

            var payment = mapper.Map<Payment>(request);
            payment.User = user;
            payment.Schedule = schedule;
            
            var paymentDetails = await PreparePaymentDetailsAsync(payment, request.PaymentDetails, cancellationToken);
            payment.PaymentDetails.Clear();
            
            payment.TotalPrice = CalculateTotalPrice(paymentDetails);
        
            Voucher? voucher = null;
            if (request.VoucherId is not null)
            {
                voucher = await voucherRepository.GetByIdAsync(request.VoucherId.Value, cancellationToken)
                              ?? throw new NotFoundException(nameof(Voucher));
                
                if (await userVoucherRepository.GetUserVoucherByUIdAndVIdAsync(user.Id, voucher.Id, cancellationToken) is not null)
                {
                    throw new BadRequestException("Voucher has already been used");
                }
                
                var voucherDiscount = voucher.Discount / 100.0;
                payment.TotalPrice -= payment.TotalPrice * voucherDiscount;
            }

            user.Point += CalculateTotalPoint(payment);
            
            if (request.Point is not null && request.Point > 100)
            {
                payment.Point = request.Point;
                user.Point -= request.Point;
                
                var pointDiscount = (double) (request.Point * 100000) / 100.0;
                payment.TotalPrice -= pointDiscount;
            }
            
            await using (var transaction = await paymentRepository.BeginTransactionAsync(cancellationToken))
            {
                try
                {
                    await paymentRepository.AddPaymentAsync(payment, cancellationToken);
                    await paymentDetailRepository.AddPaymentDetailsAsync(paymentDetails, cancellationToken);
                    
                    if (voucher is not null)
                    {
                        await UseVoucherAsync(user, voucher, cancellationToken);
                    }
                    
                    await paymentRepository.SaveChangesAsync(cancellationToken);
                    await paymentDetailRepository.SaveChangesAsync(cancellationToken);
                    await seatRepository.SaveChangesAsync(cancellationToken);
                    await foodRepository.SaveChangesAsync(cancellationToken);
                    await userRepository.SaveChangesAsync(cancellationToken);

                    await transaction.CommitAsync(cancellationToken);
                }
                catch
                {
                    await transaction.RollbackAsync(cancellationToken);
                    throw;
                }
            }

            return mapper.Map<PaymentDto>(payment);
        }

        private static double CalculateTotalPrice(IEnumerable<PaymentDetail> paymentDetails)
        {
            return paymentDetails.Sum(pd => pd.Price);
        }
        
        private static int CalculateTotalPoint(Payment payment)
        {
            return payment.TotalPrice >= 100000 ? (int) payment.TotalPrice / 10000 : 0;
        }
        
        private async Task UseVoucherAsync(User user, Voucher voucher, CancellationToken cancellationToken)
        {
            var userVoucher = new UserVoucher()
            {
                User = user,
                Voucher = voucher
            };
            
            await userVoucherRepository.AddUserVoucherAsync(userVoucher, cancellationToken);
            await userVoucherRepository.SaveChangesAsync(cancellationToken);
        }
        
        private async Task<List<PaymentDetail>> PreparePaymentDetailsAsync(
            Payment payment,
            IEnumerable<PaymentDetailDto> paymentDetailsDto,
            CancellationToken cancellationToken)
        {
            var paymentDetails = new List<PaymentDetail>();
            
            foreach (var paymentDetailDto in paymentDetailsDto)
            {
                var paymentDetail = mapper.Map<PaymentDetail>(paymentDetailDto);
                paymentDetail.Payment = payment;
                
                if (paymentDetailDto.SeatScheduleId is not null)
                {
                    var seatSchedule = await seatScheduleRepository
                        .GetSeatScheduleByIdAsync(paymentDetailDto.SeatScheduleId.Value, cancellationToken)
                         ?? throw new NotFoundException(nameof(SeatSchedule));
                    
                    switch (seatSchedule.Status)
                    {
                        case SeatSchedule.SeatScheduleStatus.Booked:
                            throw new BadRequestException("Seat is already booked");
                        case SeatSchedule.SeatScheduleStatus.Bought:
                            throw new BadRequestException("Seat is already bought");
                        case SeatSchedule.SeatScheduleStatus.Process:
                            throw new BadRequestException("Seat is in process");
                        case SeatSchedule.SeatScheduleStatus.Unavailable:
                            throw new BadRequestException("Seat is unavailable");
                    }
                    
                    if (seatSchedule.Seat.RoomId != payment.Schedule.RoomId)
                    {
                        throw new BadRequestException("Seat is not in the same room as the schedule");
                    }
                    
                    paymentDetail.SeatSchedule = seatSchedule;
                    paymentDetail.Price = seatSchedule.Seat.SeatType.Price;
                    seatSchedule.Status = SeatSchedule.SeatScheduleStatus.Bought;
                }
                
                if (paymentDetailDto.FoodId is not null)
                {
                    var food = await foodRepository.GetFoodByIdAsync(paymentDetailDto.FoodId.Value, cancellationToken)
                               ?? throw new NotFoundException(nameof(Food));
                
                    if (food.Quantity < paymentDetailDto.FoodQuantity)
                    {
                        throw new BadRequestException("Food quantity is not enough");
                    }
                
                    paymentDetail.Price = food.Price * paymentDetailDto.FoodQuantity;
                    paymentDetail.Food = food;
                    
                    food.Quantity -= paymentDetailDto.FoodQuantity;
                }
                
                paymentDetails.Add(paymentDetail);
            }

            return paymentDetails;
        }
    }
}