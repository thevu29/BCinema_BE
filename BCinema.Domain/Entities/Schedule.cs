using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BCinema.Domain.Entities
{
    [Table("Schedules")]
    public class Schedule : Base
    {
        public enum ScheduleStatus
        {
            Available,
            Ended,
            Cancelled
        }
        
        [Required]
        public DateTime Date { get; set; } = DateTime.UtcNow;
        [Required]
        public ScheduleStatus Status { get; set; } = ScheduleStatus.Available;
        [Required]
        public int MovieId { get; set; }
        [Required]
        public string MovieName { get; set; } = default!;
        [Required]
        public Guid RoomId { get; set; }
        [Required] public int Runtime { get; set; }
        
        [ForeignKey(nameof(RoomId))]
        public Room Room { get; set; } = default!;
        
        public ICollection<SeatSchedule> SeatSchedules { get; set; } = new HashSet<SeatSchedule>();
    }
}
