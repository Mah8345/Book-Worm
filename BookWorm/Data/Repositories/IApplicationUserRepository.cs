using BookWorm.Models;

namespace BookWorm.Data.Repositories
{
    public interface IApplicationUserRepository : IGenericRepository<ApplicationUser>
    {
        public Task<ApplicationUser?> GetUserByIdAsync(string id);
    }
}
