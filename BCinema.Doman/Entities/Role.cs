using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BCinema.Doman.Entities
{
    [Table("Roles")]
    public class Role : Base
    {
        [Required]
        public string Name { get; set; } = default!;
        public string? Description { get; set; } = string.Empty;

        public virtual ICollection<User> Users { get; set; } = new HashSet<User>();
    }
}
