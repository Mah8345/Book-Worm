using System.Linq.Expressions;
using BookWorm.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BookWorm.Data.Repositories
{
    public class BookRepository(ApplicationDbContext context) : GenericRepository<Book>(context), IBookRepository
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
                })
                .FirstOrDefaultAsync();
            if (result == null) return null;

            return new Book(result.Book.Title)
            {
                Id = result.Book.Id,
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

        
        public async Task<Book?> GetBookIncludeAsync<TEntity>(int id, params Expression<Func<Book, TEntity>>[] navigationProperties)
        {
            var query = Context.Books.AsQueryable();

            foreach (var navigationProperty in navigationProperties)
            {
                query.Include(navigationProperty);
            }

            return await query.FirstOrDefaultAsync(b => b.Id == id);
        }


        public async Task<IEnumerable<Book>> GetAllBooksIncludeAsync<TEntity>(
            params Expression<Func<Book, TEntity>>[] navigationProperties)
        {
            var query = Context.Books.AsQueryable();

            foreach (var navigationProperty in navigationProperties)
            {
                query.Include(navigationProperty);
            }

            return await query.ToListAsync();
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


        public async Task<IEnumerable<Book>> GetBooksWithGenreAsync(Genre genre)
        {
            var books = await Context.Books
                .Include(b => b.CoverPhoto)
                .Include(b => b.AssociatedGenres)
                .ToListAsync();
            var result = books.IsNullOrEmpty() ? [] : books.Where(b => b.AssociatedGenres.Contains(genre));
            return result;
        }


        public async Task<IEnumerable<Book>> GetBooksWithAuthorAsync(Author author)
        {
            var books = await Context.Books
                .Include(b => b.CoverPhoto)
                .Include(b => b.Authors)
                .ToListAsync();
            var result = books.IsNullOrEmpty() ? [] : books.Where(b => b.Authors.Contains(author));
            return result;
        }
    }
}
