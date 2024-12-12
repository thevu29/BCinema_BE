namespace BCinema.Application.DTOs;

public class PaymentDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string UserName { get; set; } = default!;
    public Guid ScheduleId { get; set; }
    public int MovieId { get; set; }
    public Guid? VoucherId { get; set; }
    public DateTime Date { get; set; }
    public double TotalPrice { get; set; }
    public IEnumerable<PaymentDetailDto> PaymentDetails { get; set; } = default!;
}