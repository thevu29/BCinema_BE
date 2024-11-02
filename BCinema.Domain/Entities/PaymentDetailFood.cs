using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BCinema.Domain.Entities;

[Table("PaymentDetailFood")]
public class PaymentDetailFood : Base
{
    [Required]
    public Guid PaymentDetailId { get; set; }
    [Required]
    public Guid FoodId { get; set; }
    [Required]
    public int FoodQuantity { get; set; }

    [ForeignKey(nameof(PaymentDetailId))]
    public PaymentDetail PaymentDetail { get; set; } = default!;
    [ForeignKey(nameof(FoodId))]
    public Food Food { get; set; } = default!;
}