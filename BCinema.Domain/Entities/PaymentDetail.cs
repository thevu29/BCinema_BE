using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BCinema.Domain.Entities
{
    [Table("PaymentDetails")]
    public class PaymentDetail : Base
    {
        [Required]
        public Guid PaymentId { get; set; }
        public Guid? SeatId { get; set; }
        public Guid? FoodId { get; set; }
        public int? FoodQuantity { get; set; }
        [Required]
        public double Price { get; set; }

        [ForeignKey(nameof(PaymentId))]
        public Payment Payment { get; set; } = default!;
        [ForeignKey(nameof(SeatId))]
        public Seat? Seat { get; set; }
        [ForeignKey(nameof(FoodId))]
        public Food? Food { get; set; }
    }
}
