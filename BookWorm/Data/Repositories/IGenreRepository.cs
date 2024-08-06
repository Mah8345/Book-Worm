using BookWorm.Models;

namespace BookWorm.Data.Repositories
{
    public interface IGenreRepository : IGenericRepository<Genre>
    {
        Task<IEnumerable<Genre>> SearchByNameAsync(string key);
        Task<Genre?> GetGenreWithAssociatedBooksAsync(int id);
    }
}
