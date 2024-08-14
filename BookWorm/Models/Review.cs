using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookWorm.Models
{
    public class Review
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string Title { get; set; }
        public string Body { get; set; }


        [ForeignKey("UserId")]
        public virtual ApplicationUser ReviewedBy { get; set; }

        public int BookId { get; set; }
        public virtual Book ReviewedBook { get; set; }
    }
}
