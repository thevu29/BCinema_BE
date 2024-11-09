using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Application.Helpers;
using BCinema.Domain.Entities;
using BCinema.Domain.Interfaces.IRepositories;
using MediatR;

namespace BCinema.Application.Features.Payments.Queries;

public class GetPaymentsQuery : IRequest<PaginatedList<PaymentDto>>
{
    public PaymentQuery Query { get; init; } = default!;
    
    public class GetPaymentsQueryHandler(IPaymentRepository paymentRepository, IMapper mapper)
        : IRequestHandler<GetPaymentsQuery, PaginatedList<PaymentDto>>
    {
        public async Task<PaginatedList<PaymentDto>> Handle(GetPaymentsQuery request, CancellationToken cancellationToken)
        {
            var query = paymentRepository.GetPayments();
            
            if (request.Query.UserId.HasValue)
            {
                query = query.Where(x => x.UserId == request.Query.UserId);
            }
            if (!string.IsNullOrEmpty(request.Query.Date))
            {
                query = query.FilterByDate(request.Query.Date, p => p.Date);
            }
            
            query = ApplySorting(query, request.Query.SortBy, request.Query.SortOrder);
            
            var payments = await PaginatedList<Payment>
                .ToPageList(query, request.Query.Page, request.Query.Size);

            var paymentDtos = mapper.Map<IEnumerable<PaymentDto>>(payments.Data);
            
            return new PaginatedList<PaymentDto>(request.Query.Page, request.Query.Size, payments.TotalElements, paymentDtos);
        }
        
        private static IQueryable<Payment> ApplySorting(IQueryable<Payment> query, string sortBy, string sortOrder)
        {
            var allowedSortColumns = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                nameof(Payment.Date)
            };
            
            if (string.IsNullOrWhiteSpace(sortBy) || !allowedSortColumns.Contains(sortBy))
            {
                return query.OrderByDescending(p => p.Date);
            }

            return query.ApplyDynamicSorting(sortBy, sortOrder);
        }
    }
}