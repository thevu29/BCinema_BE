﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BCinema.Domain.Entities
{
    [Table("Tokens")]
    public class Token : Base
    {
        [Required]
        public string RefreshToken { get; set; } = default!;
        [Required]
        public DateTime RefreshTokenExpireAt { get; set; }
        [Required]
        public Guid UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; } = default!;
    }
}
