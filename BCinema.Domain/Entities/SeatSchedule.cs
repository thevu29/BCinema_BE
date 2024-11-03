using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BCinema.Domain.Entities;

[Table("SeatSchedule")]
public class SeatSchedule : Base
{
    public enum SeatScheduleStatus
    {
        Available,
        Pending,
        Booked,
        Unavailable
    }
    
    [Required]
    public Guid SeatId { get; set; }
    [Required]
    public Guid ScheduleId { get; set; }
    [Required]
    public SeatScheduleStatus Status { get; set; } = SeatScheduleStatus.Available;
    
    [ForeignKey(nameof(SeatId))]
    public Seat Seat { get; set; } = default!;
    [ForeignKey(nameof(ScheduleId))]
    public Schedule Schedule { get; set; } = default!;
    
    public ICollection<PaymentDetail> PaymentDetails { get; set; } = new HashSet<PaymentDetail>();
}