using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BCinema.Domain.Entities
{
    [Table("Tokens")]
    public class Token : Base
    {
        [Required]
        public required string RefreshToken { get; set; }
        [Required]
        public required DateTime RefreshTokenExpireAt { get; set; }
        [Required]
        public required Guid UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; } = default!;
    }
}
