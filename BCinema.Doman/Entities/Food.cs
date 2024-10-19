using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BCinema.Domain.Entities
{
    [Table("Foods")]
    public class Food : Base
    {
        [Required]
        public string Name { get; set; } = default!;
        [Required]
        public double Price { get; set; } = default!;
        [Required]
        public int Quantity { get; set; } = default!;
        public DateTime? DeleteAt { get; set; }

        public virtual ICollection<PaymentDetail> PaymentDetails { get; set; } = new HashSet<PaymentDetail>();
    }
}
