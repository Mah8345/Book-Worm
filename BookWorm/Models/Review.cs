using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookWorm.Models
{
    public class Review
    {
        public int Id { get; private set; }

        [MaxLength(100)]
        public required string Title { get; set; }

        [MaxLength(10000)]
        public required string Body { get; set; }


        [ForeignKey("UserId")]
        public virtual required ApplicationUser ReviewedBy { get; set; }

        public int BookId { get; set; }
        public virtual required Book ReviewedBook { get; set; }
    }
}
