using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCinema.Application.DTOs
{
    public class SeatTypeDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public double Price { get; set; } = default!;
        public DateTime CreateAt { get; set; }
        public DateTime DeleteAt { get; set; }
    }
}
