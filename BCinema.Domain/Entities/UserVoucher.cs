﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BCinema.Domain.Entities
{
    [Table("UserVouchers")]
    public class UserVoucher : Base
    {
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public Guid VoucherId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; } = default!;
        [ForeignKey(nameof(VoucherId))]
        public Voucher Voucher { get; set; } = default!;
    }
}
