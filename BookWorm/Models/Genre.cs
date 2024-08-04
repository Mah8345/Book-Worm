using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookWorm.Models
{
    public class Genre
    {
        public int Id { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }

        [ForeignKey("ImageId")]
        public ApplicationImage Photo { get; set; }
        public ICollection<Book> AssociatedBooks { get; set; }
    }
}
