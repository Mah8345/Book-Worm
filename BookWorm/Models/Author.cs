using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookWorm.Models
{
    public class Author
    {
        public int Id { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }
        public string NormalizedName { get; set; }

        [MaxLength(1000)]
        public string? About { get; set; }

        [ForeignKey("ImageId")]
        public ApplicationImage? Photo { get; set; }
        public ICollection<Book> WrittenBooks { get; set; }
    }
}
