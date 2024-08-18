using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookWorm.Data;
using BookWorm.Data.Repositories;
using BookWorm.Models;
using BookWorm.Services;
using BookWorm.Tests.TestHelpers;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace BookWorm.Tests.ServicesTest
{
    public class BookRecommenderTests
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly ApplicationDbContext _context;
        private readonly BookRecommender _bookRecommender;


        public BookRecommenderTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: $"BookWormTest_{Guid.NewGuid()}")
                .Options;
            _context = new ApplicationDbContext(options);
            _bookRecommender = new BookRecommender(new BookRepository(_context));
        }


        [Fact]
        public async Task RecommendBasedOnFavoriteGenresAsync_ReturnsBooks()
        {
            // Arrange
            var genres = TestHelper.GenerateRandomGenres(5);
            var books = TestHelper.GenerateRandomBooks(10);
            books.ForEach(b => b.AssociatedGenres = genres);
            await _context.Books.AddRangeAsync(books);
            await _context.SaveChangesAsync();

            
            // Act
            var recommendedBooks = await _bookRecommender.RecommendBasedOnFavoriteGenresAsync(genres, 10);


            // Assert
            Assert.Contains(recommendedBooks, rb => books.Select(b => b.Id).Contains(rb.Id));
        }


        [Fact]
        public async Task RecommendBasedOnFavoriteAuthorsAsync_ReturnsBooks()
        {
            // Arrange
            var authors = TestHelper.GenerateRandomAuthors(5);
            var books = TestHelper.GenerateRandomBooks(10);
            books.ForEach(b => b.Authors = authors);
            await _context.Books.AddRangeAsync(books);
            await _context.SaveChangesAsync();


            // Act
            var recommendedBooks = await _bookRecommender.RecommendBasedOnFavoriteAuthorsAsync(authors, 10);


            // Assert
            Assert.Contains(recommendedBooks, rb => books.Select(b => b.Id).Contains(rb.Id));
        }


        [Fact]
        public async Task RecommendBasedOnRatingAsync_ReturnsBooks()
        {
            // Arrange
            var books = TestHelper.GenerateRandomBooks(10);
            books.ForEach(b => b.Comments = TestHelper.GenerateRandomComments(15));
            await _context.Books.AddRangeAsync(books);
            await _context.SaveChangesAsync();


            // Act
            var recommendedBooks = await _bookRecommender.RecommendBasedOnRatingAsync(5);


            // Assert
            var expectedBooks = books.OrderByDescending(b => b.AverageRating).Take(5).ToList();
            Assert.Equal(expectedBooks, recommendedBooks);
        }


        [Fact]
        public async Task RecommendBasedOnPopularityAsync_ReturnsBooks()
        {
            // Arrange
            var books = TestHelper.GenerateRandomBooks(10);
            var users = TestHelper.GenerateRandomUsers(10);
            users.ForEach(u => u.FavoriteBooks = books);
            await _context.ApplicationUsers.AddRangeAsync(users);
            await _context.SaveChangesAsync();


            // Act
            var recommendedBooks = await _bookRecommender.RecommendBasedOnPopularityAsync(5);


            // Assert
            var expectedBooks = books.OrderByDescending(b => b.FavoritedByUsers.Count).Take(5).ToList();
            Assert.Equal(expectedBooks, recommendedBooks);
        }


        [Fact]
        public async Task RecommendBasedOnIntroductionDate_ReturnsBooks()
        {
            // Arrange
            var books = TestHelper.GenerateRandomBooks(10);
            await _context.Books.AddRangeAsync(books);
            await _context.SaveChangesAsync();


            // Act
            var recommendedBooks = await _bookRecommender.RecommendBasedOnIntroductionDate(5);


            // Assert
            var expectedBooks = books.OrderByDescending(b => b.IntroducedAt).Take(5).ToList();
            Assert.Equal(expectedBooks, recommendedBooks);
        }
    }
}
