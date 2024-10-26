using BCinema.Doman.Entities;

namespace BCinema.Application.DTOs
{
    public class ScheduleDto
    {
        public Guid Id { get; set; }
        public Guid RoomId { get; set; }
        public string MovieId { get; set; } = default!;
        public DateTime Date { get; set; } = DateTime.Now;
        public ScheduleStatus Status { get; set; }
    }
}
