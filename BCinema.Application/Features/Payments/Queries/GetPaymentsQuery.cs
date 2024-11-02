using AutoMapper;
using BCinema.Application.DTOs;
using BCinema.Application.Exceptions;
using BCinema.Application.Helpers;
using BCinema.Domain.Entities;
using BCinema.Domain.Interfaces.IRepositories;
using MediatR;

namespace BCinema.Application.Features.Payments.Queries;

public class GetPaymentsQuery : IRequest<PaginatedList<PaymentDto>>
{
    public PaymentQuery Query { get; set; } = default!;
    
    public class GetPaymentsQueryHandler(IPaymentRepository paymentRepository, IMapper mapper)
        : IRequestHandler<GetPaymentsQuery, PaginatedList<PaymentDto>>
    {
        public async Task<PaginatedList<PaymentDto>> Handle(GetPaymentsQuery request, CancellationToken cancellationToken)
        {
            var query = paymentRepository.GetPayments();
            
            if (request.Query.UserId != null)
            {
                query = query.Where(x => x.UserId == request.Query.UserId);
            }
            if (request.Query.Date != null)
            {
                query = FilterByDate(query, request.Query.Date);
            }
            
            query = ApplySorting(query, request.Query.SortBy, request.Query.SortOrder);
            
            var payments = await PaginatedList<Payment>
                .ToPageList(query, request.Query.Page, request.Query.Size);

            var paymentDtos = mapper.Map<IEnumerable<PaymentDto>>(payments.Data);
            
            return new PaginatedList<PaymentDto>(request.Query.Page, request.Query.Size, payments.TotalElements, paymentDtos);
        }
    
        private static IQueryable<Payment> FilterByDate(IQueryable<Payment> query, string date)
        {
            if (date.Contains("to"))
            {
                var dates = date.Split("to");
                if (dates.Length == 2 &&
                    DateTime.TryParseExact(dates[0], "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.AssumeUniversal, out var startDate) &&
                    DateTime.TryParseExact(dates[1], "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.AssumeUniversal, out var endDate))
                {
                    return query.Where(x => x.Date >= startDate.ToUniversalTime() && x.Date <= endDate.ToUniversalTime());
                }
            }
            else if (date.StartsWith('>') && DateTime.TryParseExact(date[1..], "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.AssumeUniversal, out var afterDate))
            {
                return query.Where(x => x.Date > afterDate.ToUniversalTime());
            }
            else if (date.StartsWith('<') && DateTime.TryParseExact(date[1..], "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.AssumeUniversal, out var beforeDate))
            {
                return query.Where(x => x.Date < beforeDate.ToUniversalTime());
            }
            else if (DateTime.TryParseExact(date, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.AssumeUniversal, out var exactDate))
            {
                return query.Where(x => x.Date.Date == exactDate.ToUniversalTime().Date);
            }

            throw new BadRequestException("Invalid date format. Date must be in yyyy-MM-dd format");
        }
        
        private static IQueryable<Payment> ApplySorting(IQueryable<Payment> query, string sortBy, string sortOrder)
        {
            return sortBy.ToLower() switch
            {
                "date" => sortOrder.ToUpper().Equals("ASC")
                    ? query.OrderBy(x => x.Date)
                    : query.OrderByDescending(x => x.Date),
                
                _ => query.OrderByDescending(x => x.Date)
            };
        }
    }
}