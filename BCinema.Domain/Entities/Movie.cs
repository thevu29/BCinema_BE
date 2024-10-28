using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BCinema.Domain.Entities
{
    [Table("Movies")]
    public class Movie : Base
    {
        public enum MovieStatus
        {
            Released,
            ComingSoon,
            Ended,
        }
        
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

        public ICollection<MovieGenre> MovieGenres { get; } = new HashSet<MovieGenre>();
        public ICollection<Schedule> Schedules { get; } = new HashSet<Schedule>();
    }
}
