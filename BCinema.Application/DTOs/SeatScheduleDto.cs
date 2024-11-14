namespace BCinema.Application.DTOs;

public class SeatScheduleDto
{
    public Guid Id { get; set; }
    public Guid SeatId { get; set; }
    public Guid ScheduleId { get; set; }
    public double Price { get; set; }
    public string Status { get; set; } = default!;
}