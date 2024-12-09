using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BCinema.Domain.Entities
{
    [Table("Otps")]
    public class Otp : Base
    {
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public string Code { get; set; } = default!;
        public int Attempts { get; set; } = 1;
        [Required]
        public bool IsVerified { get; set; } = false;
        public DateTime ExpireAt { get; set; }
        [ForeignKey(nameof(UserId))]
        public User User { get; set; } = default!;
    }
}
