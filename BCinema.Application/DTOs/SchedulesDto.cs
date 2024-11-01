using System.Collections;

namespace BCinema.Application.DTOs
{
    public class SchedulesDto
    {
        public DateOnly Date { get; set; }
        public int Runtime { get; set; }
        public int MovieId { get; set; }
        public Guid RoomId { get; set; }
        public IEnumerable<ScheduleDetailDto> Schedules { get; set; } = default!;
    }
}
