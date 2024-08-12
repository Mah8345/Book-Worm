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
    public class AwardRepositoryTests
    {
        private readonly ApplicationDbContext _context;
        private readonly IAwardRepository _awardRepository;
        private readonly ITestOutputHelper _testOutputHelper;
        public AwardRepositoryTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "BookWormTest")
                .LogTo(_testOutputHelper.WriteLine)
                .Options;
            _context = new ApplicationDbContext(options);
            _awardRepository = new AwardRepository(_context);
        }

        [Fact]
        public async Task GetAwardWithPhotoAndBooksAsync_AwardExists_ReturnsAwardWithPhotoAndBooks()
        {
            // Arrange
            var awardedBooks = TestHelper.GenerateRandomBooks(10);
            var image = new ApplicationImage
            {
                FileName = "image.jpg",
                FileSize = 1024
            };
            var award = new Award
            {
                Name = "Award1",
                About = "About Award1",
                AwardPhoto = image,
                AwardedBooks = awardedBooks
            };
            _context.Awards.Add(award);
            await _context.SaveChangesAsync();

            // Act
            var result = await _awardRepository.GetAwardWithPhotoAndBooksAsync(award.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(award.Id, result.Id);
            foreach(var book in awardedBooks)
            {
                Assert.Contains(result.AwardedBooks, b => b.Id == book.Id);
            }
        }

        [Fact]
        public async Task GetAwardWithPhotoAndBooksAsync_AwardsExist_ReturnsAwards()
        {
            // Arrange
            var awards = TestHelper.GenerateRandomAwards(10);
            
            
            await _context.Awards.AddRangeAsync(awards);
            await _context.SaveChangesAsync();

            // Act
            var result = await _awardRepository.GetAllAwardsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            foreach (var award in awards)
            {
                Assert.Contains(result, a => a.Id == award.Id);
            }
        }

        [Fact]
        public async Task GetAllAwardsAsync_NoAwards_ReturnsEmptyList()
        {
            // Arrange

            // Act
            var result = await _awardRepository.GetAllAwardsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}
