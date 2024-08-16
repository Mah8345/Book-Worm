using BookWorm.Data.UnitOfWork;
using BookWorm.Data;
using BookWorm.Models;
using BookWorm.Services;
using BookWorm.Tests.TestHelpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace BookWorm.Tests.ServicesTest
{
    public class UserBookServiceTests
    {
        private ITestOutputHelper _testOutputHelper;
        private readonly UserBookService _userBookService;
        private readonly ApplicationDbContext _context;
        public UserBookServiceTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "BookWormTest")
                .Options;
            var context = new ApplicationDbContext(options);
            _context = context;
            var unitOfWork = new UnitOfWork(context);
            _userBookService = new UserBookService(unitOfWork);
        }


        [Fact]
        public async Task AddBookToFavoritesAsync_BookDoesNotAlreadyExist_BookIsAdded()
        {
            //arrange
            var user = TestHelper.GenerateRandomUsers(1).First();
            var book = TestHelper.GenerateRandomBooks(1).First();
            _context.ApplicationUsers.Add(user);
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
            await _userBookService.AddBookToFavoritesAsync(user.Id, book.Id);

            //act
            var result = await _context.ApplicationUsers
                .Include(u => u.FavoriteBooks)
                .FirstOrDefaultAsync(u => u.Id == user.Id);

            //assert
            Assert.NotNull(result);
            Assert.Contains(result.FavoriteBooks, b => b.Id == book.Id);
        }


        [Fact]
        public async Task AddBookToFavoritesAsync_BookAlreadyExists_ThrowsArgumentException()
        {
            //arrange
            var user = TestHelper.GenerateRandomUsers(1).First();
            var book = TestHelper.GenerateRandomBooks(1).First();
            user.FavoriteBooks = [];
            user.FavoriteBooks.Add(book);
            _context.ApplicationUsers.Add(user);
            await _context.SaveChangesAsync();

            //act
            var result = await _context.ApplicationUsers
                .Include(u => u.FavoriteBooks)
                .FirstOrDefaultAsync(u => u.Id == user.Id);

            //assert
            Assert.NotNull(result);
            await Assert.ThrowsAsync<ArgumentException>(async () =>
                await _userBookService.AddBookToFavoritesAsync(user.Id, book.Id));
        }


        [Fact]
        public async Task AddBookToReadListAsync_BookDoesNotAlreadyExist_BookIsAdded()
        {
            //arrange
            var user = TestHelper.GenerateRandomUsers(1).First();
            var book = TestHelper.GenerateRandomBooks(1).First();
            _context.ApplicationUsers.Add(user);
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
            await _userBookService.AddBookToReadListAsync(user.Id, book.Id);

            //act
            var result = await _context.ApplicationUsers
                .Include(u => u.ReadBooks)
                .FirstOrDefaultAsync(u => u.Id == user.Id);

            //assert
            Assert.NotNull(result);
            Assert.Contains(result.ReadBooks, b => b.Id == book.Id);
        }


        [Fact]
        public async Task AddBookToWantToReadListAsync_BookDoesNotAlreadyExist_BookIsAdded()
        {
            //arrange
            var user = TestHelper.GenerateRandomUsers(1).First();
            var book = TestHelper.GenerateRandomBooks(1).First();
            _context.ApplicationUsers.Add(user);
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
            await _userBookService.AddBookToWantToReadListAsync(user.Id, book.Id);

            //act
            var result = await _context.ApplicationUsers
                .Include(u => u.WantToReadBooks)
                .FirstOrDefaultAsync(u => u.Id == user.Id);

            //assert
            Assert.NotNull(result);
            Assert.Contains(result.WantToReadBooks, b => b.Id == book.Id);
        }


        [Fact]
        public async Task RemoveBookFromFavorites_BookExistsInTheList_BookIsRemoved()
        {
            //arrange
            var user = TestHelper.GenerateRandomUsers(1).First();
            var book = TestHelper.GenerateRandomBooks(1).First();
            user.FavoriteBooks = [];
            user.FavoriteBooks.Add(book);
            _context.ApplicationUsers.Add(user);
            await _context.SaveChangesAsync();


            //act
            var result = await _context.ApplicationUsers
                .Include(u => u.FavoriteBooks)
                .FirstOrDefaultAsync(u => u.Id == user.Id);


            //assert
            Assert.NotNull(result);
            Assert.True(await _userBookService.RemoveBookFromFavorites(result.Id,book.Id));
        }


        [Fact]
        public async Task RemoveBookFromFavorites_BookDoesNotExistsInTheList_ThrowsArgumentException()
        {
            //arrange
            var user = TestHelper.GenerateRandomUsers(1).First();
            var book = TestHelper.GenerateRandomBooks(1).First();
            _context.ApplicationUsers.Add(user);
            _context.Books.Add(book);
            await _context.SaveChangesAsync();


            //act
            var result = await _context.ApplicationUsers
                .Include(u => u.FavoriteBooks)
                .FirstOrDefaultAsync(u => u.Id == user.Id);


            //assert
            Assert.NotNull(result);
            await Assert.ThrowsAsync<ArgumentException>(async () => await _userBookService.RemoveBookFromFavorites(result.Id, book.Id));
        }


        [Fact]
        public async Task RemoveBookFromWantToReadList_BookDoesNotExistsInTheList_ThrowsArgumentException()
        {
            //arrange
            var user = TestHelper.GenerateRandomUsers(1).First();
            var book = TestHelper.GenerateRandomBooks(1).First();
            _context.ApplicationUsers.Add(user);
            _context.Books.Add(book);
            await _context.SaveChangesAsync();


            //act
            var result = await _context.ApplicationUsers
                .Include(u => u.WantToReadBooks)
                .FirstOrDefaultAsync(u => u.Id == user.Id);


            //assert
            Assert.NotNull(result);
            await Assert.ThrowsAsync<ArgumentException>(async () => await _userBookService.RemoveBookFromWantToReadList(result.Id, book.Id));
        }


        [Fact]
        public async Task RemoveBookFromReadList_BookDoesNotExistsInTheList_ThrowsArgumentException()
        {
            //arrange
            var user = TestHelper.GenerateRandomUsers(1).First();
            var book = TestHelper.GenerateRandomBooks(1).First();
            _context.ApplicationUsers.Add(user);
            _context.Books.Add(book);
            await _context.SaveChangesAsync();


            //act
            var result = await _context.ApplicationUsers
                .Include(u => u.ReadBooks)
                .FirstOrDefaultAsync(u => u.Id == user.Id);


            //assert
            Assert.NotNull(result);
            await Assert.ThrowsAsync<ArgumentException>(async () => await _userBookService.RemoveBookFromReadList(result.Id, book.Id));
        }
    }
}
