namespace BCinema.Application.DTOs
{
    public class SchedulesDto
    {
        public DateOnly Date { get; set; }
        public int Runtime { get; set; }
        public int MovieId { get; set; }
        public string MovieName { get; set; } = default!;
        public Guid RoomId { get; set; }
        public string RoomName { get; set; } = default!;
        public IEnumerable<ScheduleDetailDto> Schedules { get; set; } = default!;
    }
}
