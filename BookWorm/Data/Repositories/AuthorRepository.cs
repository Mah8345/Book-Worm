using BookWorm.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BookWorm.Data.Repositories
{
    public class AuthorRepository(ApplicationDbContext context) : GenericRepository<Author>(context), IAuthorRepository
    {
        public async Task<Author?> GetAuthorWithPhotoAsync(int id)
        {
            return await Context.Authors
                .Include(a => a.Photo)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public Task<Author?> GetAuthorWithPhotoAndBooksAsync(int id)
        {
            return Context.Authors
                .Include(a => a.WrittenBooks)
                .Include(a => a.Photo)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<Author>> SearchAuthorsByNameAsync(string key)
        {
            key = key.ToUpper();
            var authors = await Context.Authors
                .Include(a => a.Photo)
                .Where(a => a.NormalizedName.Contains(key))
                .ToListAsync();
            return authors.IsNullOrEmpty() ? new List<Author>() : authors;
        }
    }
}
