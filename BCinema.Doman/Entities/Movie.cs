using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BCinema.Domain.Entities
{
    public enum MovieStatus
    {
        Released,
        ComingSoon,
        Ended,
    }

    [Table("Movies")]
    public class Movie : Base
    {
        [Key] 
        public new string Id { get; set; } = default!;
        [Required]
        public string Name { get; set; } = default!;
        [Required]
        public string Description { get; set; } = default!;
        [Required]
        public string Image { get; set; } = default!;
        [Required]
        public string Country { get; set; } = default!;
        [Required]
        public Boolean Adult { get; set; } = false;
        [Required]
        public int Runtime { get; set; } = default!;
        [Required]
        public DateOnly ReleaseDate { get; set; } = default!;
        [Required]
        public MovieStatus Status { get; set; } = MovieStatus.ComingSoon;

        public virtual ICollection<MovieGenre> MovieGenres { get; set; } = new HashSet<MovieGenre>();
        public virtual ICollection<Schedule> Schedules { get; set; } = new HashSet<Schedule>();
    }
}
