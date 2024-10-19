using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BCinema.Domain.Entities
{
    [Table("PaymentDetails")]
    public class PaymentDetail : Base
    {
        [Required]
        public Guid PaymentId { get; set; }
        [Required]
        public Guid SeatId { get; set; }
        [Required]
        public Guid FoodId { get; set; }
        [Required]
        public int FoodQuantity { get; set; }
        [Required]
        public double Price { get; set; }

        [ForeignKey(nameof(PaymentId))]
        public virtual Payment Payment { get; set; } = default!;
        [ForeignKey(nameof(SeatId))]
        public virtual Seat Seat { get; set; } = default!;
        [ForeignKey(nameof(FoodId))]
        public virtual Food Food { get; set; } = default!;
    }
}
