using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookWorm.Models
{
    public class Publisher
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }
        public string NormalizedName { get; set; }

        [MaxLength(1000)]
        public string? About { get; set; }

        [ForeignKey("ImageId")]
        public virtual ApplicationImage? PublisherLogo { get; set; }
        public virtual ICollection<Book> PublishedBooks { get; set; }
    }
}
