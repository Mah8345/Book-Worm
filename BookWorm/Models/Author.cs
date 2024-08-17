using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookWorm.Models
{
    public class Author(string name)
    {
        private string _name = name;

        [Key]
        public int Id { get; private set; }

        [MaxLength(50)]
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
        public virtual ApplicationImage? Photo { get; set; }

        public virtual ICollection<Book> WrittenBooks { get; set; } = [];


        public override bool Equals(object? obj)
        {
            if (obj is Author other)
            {
                return Id == other.Id;
            }

            return false;
        }
    }
}
