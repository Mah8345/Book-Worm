using BookWorm.Models;

namespace BookWorm.Data.Repositories
{
    public interface IReviewRepository : IGenericRepository<Review>
    {
        Task<IEnumerable<Review>> GetBookReviews(int bookId);
    }
}
