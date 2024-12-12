using BCinema.Application.Interfaces;
using BCinema.Domain.Interfaces.IRepositories;
using MediatR;

namespace BCinema.Application.Features.Payments.Queries;

public class GetTotalPointsInYearOfUserQuery : IRequest<int>
{
    public Guid UserId { get; set; }
    public int Year { get; set; }
    
    public class GetTotalPointsInYearOfUserQueryHandler : IRequestHandler<GetTotalPointsInYearOfUserQuery, int>
    {
        private readonly IPaymentRepository _paymentRepository;

        public GetTotalPointsInYearOfUserQueryHandler(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public async Task<int> Handle(GetTotalPointsInYearOfUserQuery request, CancellationToken cancellationToken)
        {
            return await _paymentRepository.GetTotalPointsInYearOfUserAsync(request.UserId, request.Year, cancellationToken);
        }
    }
}