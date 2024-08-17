using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookWorm.Models
{
    public class Award
    {
        public int Id { get; private set; }

        [MaxLength(50)]
        public required string Name { get; set; }

        [MaxLength(1000)]
        public string? About { get; set; }

        [ForeignKey("ImageId")]
        public virtual ApplicationImage? AwardPhoto { get; set; }

        public virtual ICollection<Book> AwardedBooks { get; set; } = [];
    }
}
