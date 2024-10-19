using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BCinema.Domain.Entities
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
