using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BCinema.Domain.Entities
{
    [Table("Seats")]
    public class Seat : Base
    {
        public enum SeatStatus
        {
            Available,
            Pending,
            Booked,
            Unavailable
        }

        [Required]
        public string Row { get; set; } = default!;

        [Required]
        public int Number { get; set; }

        [Required]
        public SeatStatus Status { get; set; } = SeatStatus.Available;
        [Required]
        public Guid SeatTypeId { get; set; }
        [Required]
        public Guid RoomId { get; set; }

        [ForeignKey(nameof(SeatTypeId))]
        public SeatType SeatType { get; set; } = default!;
        [ForeignKey(nameof(RoomId))]
        public Room Room { get; set; } = default!;

        public ICollection<PaymentDetail> PaymentDetails { get; set; } = new HashSet<PaymentDetail>();
    }
}
