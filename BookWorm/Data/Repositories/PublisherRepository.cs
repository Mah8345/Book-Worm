using BookWorm.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BookWorm.Data.Repositories
{
    public class PublisherRepository(ApplicationDbContext context) : GenericRepository<Publisher>(context),IPublisherRepository
    {
        public async Task<IEnumerable<Publisher>> SearchByNameAsync(string key)
        {
            key = key.ToUpper();
            var publishers = await Context.Publishers
                .Include(g => g.PublisherLogo)
                .Where(g => g.NormalizedName.Contains(key))
                .ToListAsync();
            return publishers.IsNullOrEmpty() ? [] : publishers;
        }

        public async Task<Publisher?> GetPublisherWithPublishedBooksAsync(int id)
        {
            return await Context.Publishers
                .Include(g => g.PublisherLogo)
                .Include(g => g.PublishedBooks)
                .FirstOrDefaultAsync(g => g.Id == id);
        }
    }
}
