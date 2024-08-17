using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookWorm.Data;
using BookWorm.Data.Repositories;
using BookWorm.Models;
using BookWorm.Tests.TestHelpers;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace BookWorm.Tests.RepositoryTests
{
    public class PublisherRepositoryTests
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly ApplicationDbContext _context;
        private readonly PublisherRepository _publisherRepository;

        public PublisherRepositoryTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "BookWormTest")
                .Options;
            _context = new ApplicationDbContext(options);
            _publisherRepository = new PublisherRepository(_context);
        }

        [Fact]
        public async Task GetPublisherWithPublishedBooksAsync_PublisherExists_ReturnPublisher()
        {
            // Arrange
            var books = TestHelper.GenerateRandomBooks(2);
            var publisher = new Publisher("Publisher 1")
            {
                PublishedBooks = books
            };
            _context.Publishers.Add(publisher);
            await _context.SaveChangesAsync();

            // Act
            var result = await _publisherRepository.GetPublisherWithPublishedBooksAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Publisher 1", result.Name);
            Assert.Equal(2, result.PublishedBooks.Count);
            books.ForEach(book => Assert.Contains(result.PublishedBooks, b => b.Id == book.Id));
        }

        [Fact]
        public async Task SearchByNameAsync_PublishersExist_ReturnPublishers()
        {
            // Arrange
            var publishers = TestHelper.GenerateRandomPublishers(10);
            var key = publishers.First().Name[1..4];
            _context.Publishers.AddRange(publishers);
            await _context.SaveChangesAsync();

            // Act
            var result = await _publisherRepository.SearchByNameAsync(key);

            // Assert
            Assert.NotNull(result);
            result.ToList().ForEach(publisher =>

                Assert.Contains(key.ToUpper(), publisher.NormalizedName)
            );
        }
    }
}
