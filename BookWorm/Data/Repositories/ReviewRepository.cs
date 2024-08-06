using BookWorm.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BookWorm.Data.Repositories
{
    public class ReviewRepository(ApplicationDbContext context) : GenericRepository<Review>(context),IReviewRepository
    {
        public async Task<IEnumerable<Review>> GetBookReviews(int bookId)
        {
            var bookReviews = await Context.Reviews
                .Include(r => r.ReviewedBy)
                .Where(r => r.BookId == bookId)
                .ToListAsync();
            return bookReviews.IsNullOrEmpty() ? [] : bookReviews;
        }
    }
}
