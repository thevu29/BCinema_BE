namespace BCinema.Application.DTOs
{
    public class SeatDto
    {
        public Guid Id { get; set; }
        public string Row { get; set; } = default!;
        public int Number { get; set; }
        public string Status { get; set; } = default!;
        public string SeatType { get; set; } = default!;
        public string Room { get; set; } = default!;
    }
}
