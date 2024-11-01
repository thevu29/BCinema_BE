using BCinema.Application.Helpers;

namespace BCinema.Application.Features.Foods.Queries;

public class FoodQuery : PaginationQuery
{
    public string? Name { get; set; }
    public string? Price { get; set; }
    public string? Quantity { get; set; }
    
    public FoodQuery() : base() {}

    public FoodQuery(string? name, string? price, string? quantity, int page, int size) : base(page, size)
    {
        Name = name;
        Price = price;
        Quantity = quantity;
    }
}