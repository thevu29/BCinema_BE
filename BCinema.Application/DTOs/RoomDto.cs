namespace BCinema.Application.DTOs
{
    public class RoomDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
    }
}
