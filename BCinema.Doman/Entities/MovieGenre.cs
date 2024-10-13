using System.ComponentModel.DataAnnotations.Schema;

namespace BCinema.Doman.Entities
{
    [Table("MovieGenres")]
    public class MovieGenre : Base
    {
        public string MovieId { get; set; } = default!;
        public string GenreId { get; set; } = default!;

        [ForeignKey(nameof(MovieId))]
        public virtual Movie Movie { get; set; } = default!;
        [ForeignKey(nameof(GenreId))]
        public virtual Genre Genre { get; set; } = default!;
    }
}
