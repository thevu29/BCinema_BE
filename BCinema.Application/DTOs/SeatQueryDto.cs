namespace BCinema.Application.DTOs;

public class SeatQueryDto
{
    public string Room { get; set; } = default!;
    public SeatDto[] Seats { get; set; } = default!;
}