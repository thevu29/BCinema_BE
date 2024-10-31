namespace BCinema.Application.DTOs
{
    public class SeatTypeDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public double Price { get; set; }
    }
}
