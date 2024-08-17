using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookWorm.Data;
using BookWorm.Data.UnitOfWork;
using BookWorm.Services;
using BookWorm.Tests.TestHelpers;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace BookWorm.Tests.ServicesTest
{
    public class UserAuthorServiceTests
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly ApplicationDbContext _context;
        private readonly UserAuthorService _userAuthorService;

        public UserAuthorServiceTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "BookWormTest")
                .Options;
            var context = new ApplicationDbContext(options);
            _context = context;
            var unitOfWork = new UnitOfWork(context);
            _userAuthorService = new UserAuthorService(unitOfWork);
        }

        [Fact]
        public async Task AddAuthorToFavoritesAsync_AuthorDoesNotAlreadyExist_AuthorIsAdded()
        {
            //arrange
            var user = TestHelper.GenerateRandomUsers(1).First();
            var author = TestHelper.GenerateRandomAuthors(1).First();
            _context.ApplicationUsers.Add(user);
            _context.Authors.Add(author);
            await _context.SaveChangesAsync();
            await _userAuthorService.AddAuthorToFavoritesAsync(user.Id, author.Id);

            //act
            var result = await _context.ApplicationUsers
                .Include(u => u.FavoriteAuthors)
                .FirstOrDefaultAsync(u => u.Id == user.Id);

            //assert
            Assert.NotNull(result);
            Assert.Contains(result.FavoriteAuthors, a => a.Id == author.Id);
        }


        [Fact]
        public async Task AddAuthorToFavoritesAsync_AuthorAlreadyExists_ThrowsArgumentException()
        {
            //arrange
            var user = TestHelper.GenerateRandomUsers(1).First();
            var author = TestHelper.GenerateRandomAuthors(1).First();
            user.FavoriteAuthors.Add(author);
            _context.ApplicationUsers.Add(user);
            _context.Authors.Add(author);
            await _context.SaveChangesAsync();

            //act and assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await _userAuthorService.AddAuthorToFavoritesAsync(user.Id, author.Id));
        }


        [Fact]
        public async Task RemoveAuthorFromFavorites_AuthorExists_AuthorIsRemoved()
        {
            //arrange
            var user = TestHelper.GenerateRandomUsers(1).First();
            var author = TestHelper.GenerateRandomAuthors(1).First();
            user.FavoriteAuthors.Add(author);
            _context.ApplicationUsers.Add(user);
            _context.Authors.Add(author);
            await _context.SaveChangesAsync();

            //act
            var result = await _context.ApplicationUsers
                .Include(u => u.FavoriteAuthors)
                .FirstOrDefaultAsync(u => u.Id == user.Id);

            //assert
            Assert.NotNull(result);
            Assert.True(await _userAuthorService.RemoveAuthorFromFavorites(user.Id, author.Id));
        }
    }
}
