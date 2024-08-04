using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookWorm.Models
{
    public class Comment
    {
        public int Id { get; set; }

        [MaxLength(400)]
        public string? Body { get; set; }

        public int Rating { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser CommentedBy { get; set; }
    }
}
