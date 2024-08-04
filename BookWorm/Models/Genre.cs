using System.ComponentModel.DataAnnotations;

namespace BookWorm.Models
{
    public class Genre
    {
        public int Id { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }
        public ICollection<Book> AssociatedBooks { get; set; }
    }
}
