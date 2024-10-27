using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BCinema.Domain.Entities
{
    public enum ScheduleStatus
    {
        NowPlaying,
        ComingSoon,
        Ended,
        Cancelled
    }

    [Table("Schedules")]
    public class Schedule : Base
    {
        [Required]
        public DateTime Date { get; set; } = DateTime.Now;
        [Required]
        public ScheduleStatus Status { get; set; } = ScheduleStatus.ComingSoon;
        [Required]
        public string MovieId { get; set; } = default!;
        [Required]
        public Guid RoomId { get; set; } = default!;

        [ForeignKey(nameof(MovieId))]
        public Movie Movie { get; set; } = default!;
        [ForeignKey(nameof(RoomId))]
        public Room Room { get; set; } = default!;
    }
}
