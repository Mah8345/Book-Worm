using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace BookWorm.Models
{
    //todo:consider a partial view to display users for comments, probably will be used for other places too.
    //todo:remember to display Username as nickname 
    public class ApplicationUser : IdentityUser
    {

        [MaxLength(50)]
        public string FirstName { get; set; }

        [MaxLength(50)]
        public string LastName { get; set; }

        public DateTime? DateOfBirth { get; set; }
        public int? Age
        {
            get
            {
                if (!DateOfBirth.HasValue)
                {
                    return null;
                }
                var today = DateTime.Today;
                var age = DateOfBirth.Value.Year - today.Year;
                return (today.AddYears(-age) > DateOfBirth) ? age : age - 1;
            }
        }

        [MaxLength(80)]
        public string? Bio { get; set; }

        [ForeignKey("ImageId")]
        public ApplicationImage? ProfilePhoto { get; set; }

        public ICollection<Comment> Comments { get; set; }
        public ICollection<Genre> FavoriteGenres { get; set; }
        public ICollection<Author> FavoriteAuthors { get; set; }
        public ICollection<Review> Reviews { get; set; }
        //book related properties
        public ICollection<Book> FavoriteBooks { get; set; }
        public ICollection<Book> WantToReadBooks { get; set; }
        public ICollection<Book> ReadBooks { get; set; }

    }
}

