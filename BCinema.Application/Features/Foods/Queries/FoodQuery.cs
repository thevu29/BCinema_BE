using BCinema.Application.Helpers;

namespace BCinema.Application.Features.Foods.Queries
{
    public class FoodQuery : PaginationQuery
    {
        public string? Name { get; set; }

        public FoodQuery() : base()
        {
        }

        public FoodQuery(int page, int size, string Name) : base(page, size)
        {
            this.Name = Name;
        }
    }
}
