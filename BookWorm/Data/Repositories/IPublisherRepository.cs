using BookWorm.Models;

namespace BookWorm.Data.Repositories
{
    public interface IPublisherRepository : IGenericRepository<Publisher>
    {
        Task<IEnumerable<Publisher>> SearchByNameAsync(string key);
        Task<Publisher?> GetPublisherWithPublishedBooksAsync(int id);
    }
}
