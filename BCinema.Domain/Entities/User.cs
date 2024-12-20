﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BCinema.Domain.Entities
{
    [Table("Users")]
    public class User : Base
    {
        [Required]
        public string Name { get; set; } = default!;
        [Required]
        public string Email { get; set; } = default!;
        public string? Password { get; set; } = default!;
        public string? Avatar { get; set; }
        public int? Point { get; set; } = 0;
        public bool? IsActivated { get; set; } = false;
        public string? Provider { get; set; }
        [Required]
        public Guid RoleId { get; set; }
        public DateTime? DeleteAt { get; set; }

        [ForeignKey(nameof(RoleId))]
        public Role Role { get; set; } = default!;

        public ICollection<UserVoucher> UserVouchers { get; set; } = new HashSet<UserVoucher>();
        public ICollection<Token> Tokens { get; set; } = new HashSet<Token>();
        public ICollection<Payment> Payments { get; set; } = new HashSet<Payment>();
        public ICollection<Otp> Otps { get; set; } = new HashSet<Otp>();
    }
}
