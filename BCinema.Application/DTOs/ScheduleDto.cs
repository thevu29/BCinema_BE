namespace BCinema.Application.DTOs;

public class ScheduleDto
{
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public string Status { get; set; } = default!;
    public int MovieId { get; set; }
    public string MovieName{ get; set; } = default!;
    public Guid RoomId { get; set; }
    public string RoomName { get; set; } = default!;
    public int Runtime { get; set; }
}