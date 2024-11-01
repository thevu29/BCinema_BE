namespace BCinema.Application.DTOs;

public class FoodDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public double Price { get; set; }
    public int Quantity { get; set; }
    public DateTime CreateAt { get; set; }
}