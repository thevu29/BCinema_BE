using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BCinema.Domain.Entities
{
    [Table("Vouchers")]
    public class Voucher : Base
    {
        [Required]
        public string Code { get; set; } = default!;

        [Required]
        public int Discount { get; set; } = 0;

        public string? Description { get; set; }

        [Required]
        public DateTime ExpireAt { get; set; }

        public virtual ICollection<UserVoucher> UserVouchers { get; set; } = new HashSet<UserVoucher>();
        public virtual ICollection<Payment> Payments { get; set; } = new HashSet<Payment>();
    }
}
