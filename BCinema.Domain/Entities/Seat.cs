using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BCinema.Domain.Entities
{
    [Table("Seats")]
    public class Seat : Base
    {
        [Required]
        public string Row { get; set; } = default!;
        [Required]
        public int Number { get; set; }
        [Required]
        public Guid SeatTypeId { get; set; }
        [Required]
        public Guid RoomId { get; set; }

        [ForeignKey(nameof(SeatTypeId))]
        public SeatType SeatType { get; set; } = default!;
        [ForeignKey(nameof(RoomId))]
        public Room Room { get; set; } = default!;
        
        public ICollection<SeatSchedule> SeatSchedules { get; set; } = new HashSet<SeatSchedule>();
    }
}
