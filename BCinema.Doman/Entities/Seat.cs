using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BCinema.Domain.Entities
{
    public enum SeatStatus
    {
        Available,
        Pending,
        Booked,
        NotAvailable
    }

    [Table("Seats")]
    public class Seat : Base
    {
        [Required]
        public string Row { get; set; } = default!;

        [Required]
        public int Number { get; set; } = default!;

        [Required]
        public SeatStatus Status { get; set; } = SeatStatus.Available;
        [Required]
        public Guid SeatTypeId { get; set; }
        [Required]
        public Guid RoomId { get; set; }

        [ForeignKey(nameof(SeatTypeId))]
        public virtual SeatType SeatType { get; set; } = default!;
        [ForeignKey(nameof(RoomId))]
        public virtual Room Room { get; set; } = default!;

        public virtual ICollection<PaymentDetail> PaymentDetails { get; set; } = new HashSet<PaymentDetail>();
    }
}
