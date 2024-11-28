using BCinema.Domain.Interfaces.IRepositories;
using MediatR;

namespace BCinema.Application.Features.Payments.Queries;

public class GetStatisticsRevenueQuery : IRequest<double>
{
    public int Year { get; set; }
    public int Month { get; set; }
    
    public class GetStatisticsRevenueQueryHandler : IRequestHandler<GetStatisticsRevenueQuery, double>
    {
        private readonly IPaymentRepository paymentRepository;

        public GetStatisticsRevenueQueryHandler(IPaymentRepository paymentRepository)
        {
            this.paymentRepository = paymentRepository;
        }

        public async Task<double> Handle(GetStatisticsRevenueQuery request, CancellationToken cancellationToken)
        {
            return await paymentRepository.GetStatisticsRevenueAsync(request.Year, request.Month, cancellationToken);
        }
    }
}