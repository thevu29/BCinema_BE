using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BCinema.Domain.Entities
{
    [Table("Rooms")]
    public class Room : Base
    {
        [Required]
        public string Name { get; set; } = default!;
        [Required]
        public string? Description { get; set; }

        public virtual ICollection<Seat> Seats { get; set; } = new HashSet<Seat>();
        public virtual ICollection<Schedule> Schedules { get; set; } = new HashSet<Schedule>();
    }
}
