namespace BCinema.Application.DTOs;

public class ScheduleDetailDto
{
    public Guid Id { get; set; }
    public TimeSpan Time { get; set; }
    public string Status { get; set; } = default!;
}