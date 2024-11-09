using BCinema.Application.Helpers;

namespace BCinema.Application.Features.SeatTypes.Queries
{
    public class SeatTypeQuery : PaginationQuery
    {
        public string? Name { get; set; }
        public string? Price { get; set; }

        public SeatTypeQuery() : base() {}

        public SeatTypeQuery(int page, int size, string? name, string? price) : base(page, size)
        {
            Name = name;
            Price = price;
        }
    }
}
