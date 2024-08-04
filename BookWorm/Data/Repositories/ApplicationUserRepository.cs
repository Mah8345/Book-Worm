using BookWorm.Models;
using Microsoft.EntityFrameworkCore;

namespace BookWorm.Data.Repositories
{
    public class ApplicationUserRepository(ApplicationDbContext applicationDbContext) 
        : GenericRepository<ApplicationUser>(applicationDbContext), 
        IApplicationUserRepository
    {
        //it will be used to customize homepage based on the included properties
        public async Task<ApplicationUser?> GetUserByIdAsync(string id)
        {
            var user = await Context.ApplicationUsers
                .Include(u => u.ProfilePhoto)
                .Include(u => u.FavoriteAuthors)
                .Include(u => u.FavoriteGenres)
                .Include(u => u.FavoriteBooks)
                .FirstOrDefaultAsync(u => u.Id == id);
            return user;
        }
    }
}
