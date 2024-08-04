using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookWorm.Models
{
    public class Book
    {
        public int Id { get; set; }

        [MaxLength(150)]
        public string Title { get; set; }
        public string Introduction { get; set; }


        [MaxLength(Int32.MaxValue)]
        public string? Summary { get; set; }

        public int? PagesNumber { get; set; }
        public string AverageRating => Comments.Average(c => Convert.ToDouble(c.Rating)).ToString("F1");

        [ForeignKey("ImageId")]
        public ApplicationImage? CoverPhoto { get; set; }

        [ForeignKey("PublisherId")]
        public Publisher? Publisher { get; set; }

        public ICollection<Author> Authors { get; set; }
        public ICollection<Genre> AssociatedGenres { get; set; }
        public ICollection<Award> Awards { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public ICollection<Comment> Comments { get; set; }

        //self referencing relationships
        public ICollection<Book> SimilarBooks { get; set; }

        //user related properties
        //public ICollection<ApplicationUser> FavoritedByUsers { get; set; }
        //public ICollection<ApplicationUser> WantToReadUsers { get; set; }
        //public ICollection<ApplicationUser> ReadByUsers { get; set; }
    }
}
