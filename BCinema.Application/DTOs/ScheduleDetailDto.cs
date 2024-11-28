namespace BCinema.Application.DTOs;

public class ScheduleDetailDto
{
    public Guid Id { get; set; }
    public string Time { get; set; } = default!;
    public string Status { get; set; } = default!;
}