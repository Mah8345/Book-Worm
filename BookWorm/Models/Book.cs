using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.IdentityModel.Tokens;

namespace BookWorm.Models
{
    public class Book(string title)
    {
        private string _title = title;
        public int Id { get; set; }

        [MaxLength(40)] 
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                NormalizedTitle = value.ToUpper();
            }
        }

        public string NormalizedTitle { get; private set; } = title.ToUpper();

        [MaxLength(1000)]
        public required string Introduction { get; set; }


        [MaxLength(Int32.MaxValue)]
        public string? Summary { get; set; }

        public int? PagesNumber { get; set; }
        public string AverageRating => Ratings.IsNullOrEmpty() ? 
            "Be the First to Rate" :
            Ratings.Average(r => Convert.ToDouble(r.Score)).ToString("F1");

        public DateTime IntroducedAt { get; set; } = DateTime.Now;

        [ForeignKey("ImageId")]
        public virtual ApplicationImage? CoverPhoto { get; set; }

        [ForeignKey("PublisherId")]
        public virtual Publisher? Publisher { get; set; }

        public virtual ICollection<Author> Authors { get; set; } = [];
        public virtual ICollection<Genre> AssociatedGenres { get; set; } = [];
        public virtual ICollection<Award> Awards { get; set; } = [];
        public virtual ICollection<Review> Reviews { get; set; } = [];
        public virtual ICollection<Comment> Comments { get; set; } = [];

        public virtual ICollection<Rating> Ratings { get; set; } = [];

        public virtual ICollection<ApplicationUser> FavoritedByUsers { get; set; } = [];

        //self referencing relationships
        public virtual ICollection<Book> SimilarBooks { get; set; } = [];
    }
}
