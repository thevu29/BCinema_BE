using BCinema.Application.Helpers;

namespace BCinema.Application.Features.SeatTypes.Queries
{
    public class SeatTypeQuery : PaginationQuery
    {
        public string? Search { get; set; }
        public string? Price { get; set; }

        public SeatTypeQuery() : base() {}

        public SeatTypeQuery(int page, int size, string? search, string? price) : base(page, size)
        {
            Search = search;
            Price = price;
        }
    }
}
