using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookWorm.Models
{
    public class Award
    {
        public int Id { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(1000)]
        public string? About { get; set; }

        [ForeignKey("ImageId")]
        public ApplicationImage? AwardPhoto { get; set; }
        public ICollection<Book> AwardedBooks { get; set; }
    }
}
