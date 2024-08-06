using BookWorm.Models;

namespace BookWorm.Data.Repositories
{
    public interface IAwardRepository : IGenericRepository<Award>
    {
        Task<Award?> GetAwardWithPhotoAndBooksAsync(int id);

        Task<IEnumerable<Award>> GetAllAwardsAsync();
    }
}
