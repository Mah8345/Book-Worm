using System.Linq.Expressions;
using BookWorm.Models;

namespace BookWorm.Data.Repositories
{
    public interface IApplicationUserRepository : IGenericRepository<ApplicationUser>
    {
        Task<ApplicationUser?> GetUserByIdAsync(string id);

        //selective eager loading
        Task<ApplicationUser?> GetUserWith<TEntity>(string id,
            params Expression<Func<ApplicationUser, TEntity>>[] navigationProperties);
    }
}
