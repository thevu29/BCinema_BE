using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BCinema.Domain.Entities
{
    [Table("Payments")]
    public class Payment : Base
    {
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public Guid ScheduleId { get; set; }
        [Required]
        public Guid VoucherId { get; set; }
        [Required]
        public DateTime Date { get; set; } = DateTime.Now;
        [Required]
        public double TotalPrice { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; } = default!;
        [ForeignKey(nameof(ScheduleId))]
        public virtual Schedule Schedule { get; set; } = default!;
        [ForeignKey(nameof(VoucherId))]
        public virtual Voucher Voucher { get; set; } = default!;

        public virtual ICollection<PaymentDetail> PaymentDetails { get; set; } = new HashSet<PaymentDetail>();
    }
}
