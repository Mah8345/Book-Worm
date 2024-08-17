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
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace BookWorm.Tests.RepositoryTests
{
    public class BookRepositoryTests
    {
        private readonly ApplicationDbContext _context;
        private readonly IBookRepository _bookRepository;
        private readonly ITestOutputHelper _testOutputHelper;
        public BookRepositoryTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "BookWormTest")
                .Options;
            _context = new ApplicationDbContext(options);
            _bookRepository = new BookRepository(_context);
        }

        [Fact]
        public async Task GetBookByIdAsync_BookExists_ReturnsBook()
        {
            //Arrange
            var cover = new ApplicationImage()
            {
                FileName = "cover",
                FileSize = 1024
            };
            var comments = TestHelper.GenerateRandomComments(10);
            var genres = TestHelper.GenerateRandomGenres(10);
            var publisherImage = new ApplicationImage()
            {
                FileName = "publisher1_logo.png",
                FileSize = 1024
            };
            var publisher = new Publisher("publisher1")
            {
                About = "about1",
                PublisherLogo = publisherImage
            };
            var authors = TestHelper.GenerateRandomAuthors(10, true);
            var book = new Book("Title1")
            {
                AssociatedGenres = genres,
                Authors = authors,
                Comments = comments,
                CoverPhoto = cover,
                Introduction = "introduction1",
                Publisher = publisher,
                PagesNumber = 120
            };
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();

            //Act
            var result = await _bookRepository.GetBookByIdAsync(1);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(book.Id, result.Id);

            Assert.Equal(book.CoverPhoto.Id, result.CoverPhoto?.Id);

            Assert.NotNull(result.Comments);
            Assert.NotEmpty(result.Comments);
            foreach (var comment in comments)
            {
                Assert.Contains(result.Comments, c => c.Id == comment.Id);
                Assert.Contains(result.Comments, c => c.CommentedBy.Id == comment.CommentedBy.Id);
            }

            Assert.NotNull(result.Publisher);
            Assert.Equal(book.Publisher, result.Publisher);
            Assert.Equal(book.Publisher.PublisherLogo, result.Publisher.PublisherLogo);

            Assert.NotNull(result.Authors);
            Assert.NotEmpty(result.Authors);
            foreach (var author in authors)
            {
                Assert.Contains(result.Authors, a => a.Id == author.Id);
                Assert.Contains(result.Authors, a => a.Photo?.Id == author.Photo?.Id);
            }

            Assert.NotNull(result.AssociatedGenres);
            Assert.NotEmpty(result.AssociatedGenres);
            foreach (var genre in genres)
            {
                Assert.Contains(result.AssociatedGenres, g => g.Id == genre.Id);
                Assert.Contains(result.AssociatedGenres, g => g.Photo.Id == genre.Photo.Id);
            }
        }

        [Fact]
        public async Task GetBookByIdAsync_BookDoesNotExist_ReturnsNull()
        {
            //Arrange

            //Act
            var result = await _bookRepository.GetBookByIdAsync(2);

            //Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task SearchByNameAsync_BooksExist_ReturnsBooks()
        {
            //Arrange
            var books = TestHelper.GenerateRandomBooks(10);
            var key = books.First().Title[..3];
            await _context.Books.AddRangeAsync(books);
            await _context.SaveChangesAsync();

            //Act
            var result = await _bookRepository.SearchByNameAsync(key);

            //Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            foreach (var book in result)
            {
                Assert.True(book.NormalizedTitle.Contains(key.ToUpper()));
            }
        }
    }
}
