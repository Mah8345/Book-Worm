using BookWorm.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BookWorm.Data.Repositories
{
    public class BookRepository(ApplicationDbContext context) : GenericRepository<Book>(context),IBookRepository
    {
        public async Task<Book?> GetBookByIdAsync(int id)
        {
            var result =  await Context.Books
                .Select(b => new
                {
                    Book = b,
                    b.CoverPhoto,
                    b.Comments,
                    CommentUsers = b.Comments.Select(c => c.CommentedBy),
                    CommentUsersPhotos = b.Comments.Select(c => c.CommentedBy.ProfilePhoto),
                    b.Publisher,
                    PublisherLogo = b.Publisher != null ? b.Publisher.PublisherLogo : null,
                    b.Authors,
                    AuthorPhotos = b.Authors.Select(a => a.Photo),
                    b.AssociatedGenres,
                    GenrePhotos = b.AssociatedGenres.Select(g => g.Photo)
                })
                .FirstOrDefaultAsync();
            if (result == null) return null;

            return new Book()
            {
                Id = result.Book.Id,
                Title = result.Book.Title,
                NormalizedTitle = result.Book.NormalizedTitle,
                Introduction = result.Book.Introduction,
                Summary = result.Book.Summary,
                PagesNumber = result.Book.PagesNumber,
                CoverPhoto = result.CoverPhoto,
                Publisher = result.Publisher,
                Authors = result.Authors,
                AssociatedGenres = result.AssociatedGenres,
                Comments = result.Comments
            };
        }

        public async Task<IEnumerable<Book>> SearchByNameAsync(string key)
        {
            key = key.ToUpper();
            var books = await Context.Books
                .Include(b => b.CoverPhoto)
                .Include(b => b.Comments)
                .Where(b => b.NormalizedTitle.Contains(key))
                .ToListAsync();
            return books.IsNullOrEmpty() ? [] : books;
        }
    }
}
