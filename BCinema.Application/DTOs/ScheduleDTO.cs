using BCinema.Doman.Entities;

namespace BCinema.Application.DTOs
{
    public class ScheduleDto
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public ScheduleStatus Status { get; set; }
    }
}
