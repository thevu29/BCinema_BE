using BCinema.Application.Helpers;

namespace BCinema.Application.Features.Vouchers.Queries;

public class VoucherQuery : PaginationQuery
{
    public string? Search { get; set; }
    public string? Discount { get; set; }
    
    public VoucherQuery() : base() {}
    
    public VoucherQuery(int page, int size, string? search, string? discount) : base(page, size)
    {
        Search = search;
        Discount = discount;
    }
}