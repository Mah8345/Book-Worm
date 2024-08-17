using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookWorm.Models
{
    public class Publisher(string name)
    {
        private string _name = name;
        public int Id { get; private set; }

        [MaxLength(100)]
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                NormalizedName = value.ToUpper();
            }
        }

        public string NormalizedName { get; private set; } = name.ToUpper();

        [MaxLength(1000)]
        public string? About { get; set; }

        [ForeignKey("ImageId")]
        public virtual ApplicationImage? PublisherLogo { get; set; }

        public virtual ICollection<Book> PublishedBooks { get; set; } = [];
    }
}
