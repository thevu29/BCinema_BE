using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCinema.Application.DTOs
{
    public class FoodDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public int Quantity { get; set; } = 0;
        public double Price { get; set; } = 0;
    }
}
