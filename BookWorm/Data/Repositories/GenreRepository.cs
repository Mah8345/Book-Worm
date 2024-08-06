using BookWorm.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BookWorm.Data.Repositories
{
    public class GenreRepository(ApplicationDbContext context) : GenericRepository<Genre>(context), IGenreRepository
    {
        public async Task<IEnumerable<Genre>> SearchByNameAsync(string key)
        {
            key = key.ToUpper();
            var genres = await Context.Genres
                .Include(g => g.Photo)
                .Where(g => g.NormalizedName.Contains(key))
                .ToListAsync();
            return genres.IsNullOrEmpty() ? [] : genres;
        }

        public async Task<Genre?> GetGenreWithAssociatedBooksAsync(int id)
        {
            return await Context.Genres
                .Include(g => g.Photo)
                .Include(g => g.AssociatedBooks)
                .FirstOrDefaultAsync(g => g.Id == id);
        }
    }
}
