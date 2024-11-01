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
        public double Price { get; set; }
        [Required]
        public int Quantity { get; set; }
        public DateTime? DeleteAt { get; set; }

        public ICollection<PaymentDetail> PaymentDetails { get; } = new HashSet<PaymentDetail>();
    }
}
