using System.ComponentModel.DataAnnotations;

namespace BCinema.Domain.Entities
{
    public class Base
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime CreateAt { get; set; } = DateTime.Now;
        public DateTime? UpdateAt { get; set; }
    }
}
