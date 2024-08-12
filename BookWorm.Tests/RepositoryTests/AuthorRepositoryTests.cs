using BookWorm.Data.Repositories;
using BookWorm.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookWorm.Models;
using BookWorm.Tests.TestHelpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace BookWorm.Tests.RepositoryTests
{
    public class AuthorRepositoryTests
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly ApplicationDbContext _context;
        private readonly AuthorRepository _authorRepository;

        public AuthorRepositoryTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "BookWormTest")
                .LogTo(_testOutputHelper.WriteLine, LogLevel.Information)
                .Options;
            _context = new ApplicationDbContext(options);
            _authorRepository = new AuthorRepository(_context);
        }


        [Fact]
        public async Task GetAuthorWithPhotoAsync_AuthorExists_ReturnsAuthor()
        {
            //Arrange
            var author = TestHelper.GenerateRandomAuthors(1).First();
            var photo = new ApplicationImage()
            {
                FileName = "photo.jpg",
                FileSize = 1024
            };
            var books = TestHelper.GenerateRandomBooks(10);
            author.WrittenBooks = books;
            author.Photo = photo;
            _context.Authors.Add(author);
            await _context.SaveChangesAsync();

            //Act
            var result = await _authorRepository.GetAuthorWithPhotoAsync(1);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(author.Id, result.Id);
            Assert.Equal(author.Photo.Id, result?.Photo?.Id);
        }

        [Fact]
        public async Task GetAuthorWithPhotoAndBooksAsync_AuthorExists_ReturnsAuthor()
        {
            //Arrange
            var author = TestHelper.GenerateRandomAuthors(1).First();
            var photo = new ApplicationImage()
            {
                FileName = "photo.jpg",
                FileSize = 1024
            };
            var books = TestHelper.GenerateRandomBooks(10);
            author.WrittenBooks = books;
            author.Photo = photo;
            _context.Authors.Add(author);
            await _context.SaveChangesAsync();

            //Act
            var result = await _authorRepository.GetAuthorWithPhotoAndBooksAsync(1);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(author.Id, result.Id);
            Assert.Equal(author.Photo.Id, result?.Photo?.Id);
            foreach (var book in books)
            {
                Assert.Contains(result.WrittenBooks, b => b.Id == book.Id);
            }
        }

        [Fact]
        public async Task SearchAuthorsByNameAsync_AuthorExists_ReturnsAuthors()
        {
            //Arrange
            var authors = TestHelper.GenerateRandomAuthors(10);
            var key = authors.First().Name[..3];
            foreach (var author in authors)
            {
                _context.Authors.Add(author);
            }
            await _context.SaveChangesAsync();

            //Act
            var result = await _authorRepository.SearchAuthorsByNameAsync(key);

            //Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            foreach (var author in result)
            {
                Assert.Contains(key.ToUpper(), author.NormalizedName);
            }
        }

        [Fact]
        public async Task SearchAuthorsByNameAsync_AuthorDoesNotExist_ReturnsEmptyList()
        {
            //Arrange
            var authors = TestHelper.GenerateRandomAuthors(10);
            var key = "random key";
            foreach (var author in authors)
            {
                _context.Authors.Add(author);
            }
            await _context.SaveChangesAsync();

            //Act
            var result = await _authorRepository.SearchAuthorsByNameAsync(key);

            //Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}
