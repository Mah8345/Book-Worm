using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookWorm.Models
{
    public class Comment
    {
        public int Id { get; private set; }

        [MaxLength(400)]
        public string? Body { get; set; }

        public DateTime CommentedAt { get; } = DateTime.Now;

        [ForeignKey("UserId")]
        public virtual required ApplicationUser CommentedBy { get; set; }
    }
}
