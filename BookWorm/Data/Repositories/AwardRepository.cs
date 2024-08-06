using BookWorm.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BookWorm.Data.Repositories
{
    public class AwardRepository(ApplicationDbContext context) : GenericRepository<Award>(context), IAwardRepository
    {
        public async Task<Award?> GetAwardWithPhotoAndBooksAsync(int id)
        {
            return await Context.Awards
                .Include(a => a.AwardPhoto)
                .Include(a => a.AwardedBooks)
                .ThenInclude(b => b.CoverPhoto)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        //used for awards introduction page
        public async Task<IEnumerable<Award>> GetAllAwardsAsync()
        {
            var awards = await Context.Awards
                .Include(a => a.AwardPhoto)
                .ToListAsync();
            return awards.IsNullOrEmpty() ? new List<Award>() : awards;
        }
    }
}