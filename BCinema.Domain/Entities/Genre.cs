using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BCinema.Domain.Entities
{
    [Table("Genres")]
    public class Genre : Base
    {
        [Key]
        public new string Id { get; } = default!;
        [Required]
        public string Name { get; } = default!;

        public ICollection<MovieGenre> MovieGenres { get; } = new HashSet<MovieGenre>();
    }
}
