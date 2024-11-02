using BCinema.Application.Helpers;

namespace BCinema.Application.Features.Payments.Queries;

public class PaymentQuery : PaginationQuery
{
    public Guid? UserId { get; set; }
    public string? Date { get; set; }
    
    public PaymentQuery() : base() {}
    
    public PaymentQuery(string? date, int page, int pageSize) : base(page, pageSize)
    {
        Date = date;
    }
}