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
        public ApplicationUser ReviewedBy { get; set; }

        public int BookId { get; set; }
        public Book ReviewedBook { get; set; }
    }
}
