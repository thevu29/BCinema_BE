using System.ComponentModel.DataAnnotations;
using System.Globalization;
using BCinema.Application.Exceptions;
using BCinema.Application.Momo;
using BCinema.Domain.Entities;
using BCinema.Domain.Interfaces.IRepositories;
using MediatR;

namespace BCinema.Application.Features.Payments.Commands;

public class CreateMomoUrlCommand : IRequest<string>
{
    [Required]
    public Guid PaymentId { get; set; }
    
    public class CreateMomoUrlCommandHandler(
        IPaymentRepository paymentRepository,
        IMomoService momoService)
        : IRequestHandler<CreateMomoUrlCommand, string>
    {
        public async Task<string> Handle(CreateMomoUrlCommand request, CancellationToken cancellationToken)
        {
            var payment = await paymentRepository.GetPaymentByIdAsync(request.PaymentId, cancellationToken)
                          ?? throw new NotFoundException(nameof(Payment));
            
            var paymentId = payment.Id.ToString();
            var paymentAmount = payment.TotalPrice.ToString(CultureInfo.InvariantCulture);
            var paymentInfo = "Thanh toán vé xem phim";
            return await momoService.CreateMomoPaymentUrl(paymentId, paymentInfo, paymentAmount);
        }
    }
}