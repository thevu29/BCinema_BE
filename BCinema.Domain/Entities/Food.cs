using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BCinema.Domain.Entities
{
    [Table("Foods")]
    public class Food : Base
    {
        [Required]
        public string Name { get; } = default!;
        [Required]
        public double Price { get; } = default!;
        [Required]
        public int Quantity { get; } = default!;
        public DateTime? DeleteAt { get; set; }

        public ICollection<PaymentDetail> PaymentDetails { get; } = new HashSet<PaymentDetail>();
    }
}
