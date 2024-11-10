using BCinema.Application.Helpers;

namespace BCinema.Application.Features.Foods.Queries;

public class FoodQuery : PaginationQuery
{
    public string? Search { get; set; }
    public string? Price { get; set; }
    public string? Quantity { get; set; }
    
    public FoodQuery() : base() {}

    public FoodQuery(string? search, string? price, string? quantity, int page, int size) : base(page, size)
    {
        Search = search;
        Price = price;
        Quantity = quantity;
    }
}