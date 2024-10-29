using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BCinema.Domain.Entities
{
    [Table("Schedules")]
    public class Schedule : Base
    {
        public enum ScheduleStatus
        {
            NowPlaying,
            ComingSoon,
            Ended,
            Cancelled
        }
        
        [Required]
        public DateTime Date { get; set; } = DateTime.UtcNow;
        [Required]
        public ScheduleStatus Status { get; set; } = ScheduleStatus.ComingSoon;
        [Required]
        public int MovieId { get; set; }
        [Required]
        public Guid RoomId { get; set; }

        [Required] public int Runtime { get; set; }
        
        [ForeignKey(nameof(RoomId))]
        public Room Room { get; set; } = default!;
    }
}
