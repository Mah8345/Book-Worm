using BookWorm.Data;
using BookWorm.Data.Repositories;
using BookWorm.Models;
using BookWorm.Tests.TestHelpers;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace BookWorm.Tests.RepositoryTests
{
    public class ReviewRepositoryTests
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly ApplicationDbContext _context;
        private readonly ReviewRepository _reviewRepository;

        public ReviewRepositoryTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "BookWormTest")
                .Options;
            _context = new ApplicationDbContext(options);
            _reviewRepository = new ReviewRepository(_context);
        }

        [Fact]
        public async Task GetBookReviews_BookHasReviews_ReturnsReviews()
        {
            // Arrange
            var reviews = TestHelper.GenerateRandomReviews(10);
            var book = new Book()
            {
                Title = "Book_1",
                NormalizedTitle = "BOOK_1",
                Introduction = "introduction_1",
                Reviews = reviews
            };
            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            // Act
            var result = await _reviewRepository.GetBookReviews(book.Id);

            // Assert
            Assert.NotNull(result);
            foreach (var review in reviews)
            {
                Assert.Contains(result, r => r.Id == review.Id);
            }
        }
    }
}

