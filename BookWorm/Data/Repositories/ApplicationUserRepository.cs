using System.Linq.Expressions;
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


        
        public Task<ApplicationUser?> GetUserWith<TEntity>(string id,
            params Expression<Func<ApplicationUser, TEntity>>[] navigationProperties)
        {
            var query = Context.ApplicationUsers.AsQueryable();

            foreach (var navigationProperty in navigationProperties)
            {
                query.Include(navigationProperty);
            }

            return query.FirstOrDefaultAsync(u => u.Id == id);
        }
    }
}
