namespace BCinema.Application.DTOs;

public class SeatScheduleDto
{
    public Guid Id { get; set; }
    public Guid SeatId { get; set; }
    public Guid SeatTypeId { get; set; }
    public Guid ScheduleId { get; set; }
    public string Row { get; set; } = default!;
    public int Number { get; set; }
    public string SeatType { get; set; } = default!;
    public double Price { get; set; }
    public string Status { get; set; } = default!;
}