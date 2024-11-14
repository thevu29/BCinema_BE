using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Application.Exceptions;
using BCinema.Domain.Interfaces.IRepositories;
using MediatR;

namespace BCinema.Application.Features.Payments.Queries;

public class GetPaymentByIdQuery : IRequest<PaymentDto>
{
    public Guid Id { get; set; }

    public class GetPaymentByIdHandler(IPaymentRepository paymentRepository, IMapper mapper) 
        : IRequestHandler<GetPaymentByIdQuery, PaymentDto>
    {
        public async Task<PaymentDto> Handle(GetPaymentByIdQuery request, CancellationToken cancellationToken)
        {
            var payment = await paymentRepository.GetPaymentByIdAsync(request.Id, cancellationToken)
                ?? throw new NotFoundException("Payment");
                
            return mapper.Map<PaymentDto>(payment);
        }
    }
}