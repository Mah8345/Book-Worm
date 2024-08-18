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
    public class GenreRepositoryTests
    {
        private ITestOutputHelper _testOutputHelper;
        private ApplicationDbContext _context;
        private GenreRepository _genreRepository;
        public GenreRepositoryTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: $"BookWormTest_{Guid.NewGuid()}")
                .Options;
            _context = new ApplicationDbContext(options);
            _genreRepository = new GenreRepository(_context);
        }


        [Fact]
        public async Task GetGenreWithAssociatedBooksAsync_GenreExists_ReturnGenre()
        {
            //Arrange
            var associatedBooks = TestHelper.GenerateRandomBooks(10);
            var genre = new Genre("Genre1")
            {
                AssociatedBooks = associatedBooks,
                Photo = new ApplicationImage()
                {
                    FileName = "NAME",
                    FileSize = 1024
                }
            };
            await _context.Genres.AddAsync(genre);
            _testOutputHelper.WriteLine(genre.Id.ToString());
            await _context.SaveChangesAsync();

            //Act
            var result = await _genreRepository.GetGenreWithAssociatedBooksAsync(1);

            //Assert
            Assert.NotNull(result);
            Assert.NotNull(result.AssociatedBooks);
            foreach (var book in associatedBooks)
            {
                Assert.Contains(result.AssociatedBooks, b => b.Id == book.Id);
            }
        }

        [Fact]
        public async Task SearchByNameAsync_BooksExist_ReturnBooks()
        {
            //Arrange
            var genres = TestHelper.GenerateRandomGenres(10);
            var key = genres.First().Name[..3];
            await _context.Genres.AddRangeAsync(genres);
            await _context.SaveChangesAsync();

            //Act
            var result = await _genreRepository.SearchByNameAsync(key);

            //Assert
            Assert.Contains(result, g => g.NormalizedName.Contains(key.ToUpper()));
        }
    }
}
