namespace BCinema.Application.DTOs;

public class VoucherDto
{
    public Guid Id { get; set; }
    public string Code { get; set; } = default!;
    public int Discount { get; set; }
    public string? Description { get; set; }
    public DateTime CreateAt { get; set; }
    public DateTime ExpireAt { get; set; }
    
}