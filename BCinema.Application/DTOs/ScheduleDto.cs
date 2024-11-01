namespace BCinema.Application.DTOs;

public class ScheduleDto
{
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public string Status { get; set; } = default!;
    public int MovieId { get; set; }
    public Guid RoomId { get; set; }
    public int Runtime { get; set; }
}