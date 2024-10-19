using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BCinema.Domain.Entities
{
    [Table("Genres")]
    public class Genre : Base
    {
        [Key]
        public new string Id { get; set; } = default!;
        [Required]
        public string Name { get; set; } = default!;

        public virtual ICollection<MovieGenre> MovieGenres { get; set; } = new HashSet<MovieGenre>();
    }
}
