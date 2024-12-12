using BCinema.Application.Helpers;

namespace BCinema.Application.Features.Payments.Queries;

public class PaymentQuery : PaginationQuery
{
    public string? Search { get; set; }
    public Guid? UserId { get; set; }
    public string? Date { get; set; }
    public bool? HasPoint { get; set; }
    
    public PaymentQuery() : base() {}
    
    public PaymentQuery(string? search, Guid? userId, string? date, int page, int pageSize) : base(page, pageSize)
    {
        Search = search;
        UserId = userId;
        Date = date;
    }
}