using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BCinema.Doman.Entities
{
    [Table("SeatTypes")]
    public class SeatType : Base
    {
        [Required]
        public string Name { get; set; } = default!;
        [Required]
        public double Price { get; set; } = default!;

        public virtual ICollection<Seat> Seats { get; set; } = new HashSet<Seat>();
    }
}
