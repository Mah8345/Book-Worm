using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookWorm.Models
{
    public class Book
    {
        public int Id { get; set; }

        [MaxLength(150)]
        public string Title { get; set; }
        public string NormalizedTitle { get; set; }
        public string Introduction { get; set; }


        [MaxLength(Int32.MaxValue)]
        public string? Summary { get; set; }

        public int? PagesNumber { get; set; }
        public string AverageRating => Comments.Average(c => Convert.ToDouble(c.Rating)).ToString("F1");

        [ForeignKey("ImageId")]
        public virtual ApplicationImage? CoverPhoto { get; set; }

        [ForeignKey("PublisherId")]
        public virtual Publisher? Publisher { get; set; }

        public virtual ICollection<Author> Authors { get; set; }
        public virtual ICollection<Genre> AssociatedGenres { get; set; }
        public virtual ICollection<Award> Awards { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }

        //self referencing relationships
        public virtual ICollection<Book> SimilarBooks { get; set; }
    }
}
