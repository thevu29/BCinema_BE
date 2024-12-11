namespace BCinema.Application.DTOs;

public class UseVoucherDto
{
    public Guid VoucherId { get; set; }
    public Guid UserId { get; set; }
    public bool IsUsed { get; set; }
}