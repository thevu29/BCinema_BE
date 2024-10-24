using BCinema.Application.Helpers;

namespace BCinema.Application.Features.Vouchers.Queries;

public class VoucherQuery : PaginationQuery
{
    public string? Code { get; set; }
    
    public VoucherQuery() : base()
    {
    }
    
    public VoucherQuery(int page, int size, string code) : base(page, size)
    {
        Code = code;
    }
}