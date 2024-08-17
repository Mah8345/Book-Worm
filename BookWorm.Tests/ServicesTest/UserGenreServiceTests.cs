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
    public class UserGenreServiceTests
    {
        private ITestOutputHelper _testOutputHelper;
        private readonly UserGenreService _userGenreService;
        private readonly ApplicationDbContext _context;
        public UserGenreServiceTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "BookWormTest")
                .Options;
            var context = new ApplicationDbContext(options);
            _context = context;
            var unitOfWork = new UnitOfWork(context);
            _userGenreService = new UserGenreService(unitOfWork);
        }


        [Fact]
        public async Task AddGenreToFavoritesAsync_GenreDoesNotAlreadyExist_GenreIsAdded()
        {
            //arrange
            var user = TestHelper.GenerateRandomUsers(1).First();
            var genre = TestHelper.GenerateRandomGenres(1).First();
            _context.ApplicationUsers.Add(user);
            _context.Genres.Add(genre);
            await _context.SaveChangesAsync();
            await _userGenreService.AddGenreToFavoritesAsync(user.Id, genre.Id);

            //act
            var result = await _context.ApplicationUsers
                .Include(u => u.FavoriteGenres)
                .FirstOrDefaultAsync(u => u.Id == user.Id);

            //assert
            Assert.NotNull(result);
            Assert.Contains(result.FavoriteGenres, g => g.Id == genre.Id);
        }


        [Fact]
        public async Task AddGenreToFavoritesAsync_GenreAlreadyExists_ThrowsArgumentException()
        {
            //arrange
            var user = TestHelper.GenerateRandomUsers(1).First();
            var genre = TestHelper.GenerateRandomGenres(1).First();
            user.FavoriteGenres.Add(genre);
            _context.ApplicationUsers.Add(user);
            _context.Genres.Add(genre);
            await _context.SaveChangesAsync();

            //act
            async Task Act() => await _userGenreService.AddGenreToFavoritesAsync(user.Id, genre.Id);

            //assert
            await Assert.ThrowsAsync<ArgumentException>(Act);
        }


        [Fact]
        public async Task RemoveGenreFromFavorites_GenreExists_GenreIsRemoved()
        {
            //arrange
            var user = TestHelper.GenerateRandomUsers(1).First();
            var genre = TestHelper.GenerateRandomGenres(1).First();
            user.FavoriteGenres.Add(genre);
            _context.ApplicationUsers.Add(user);
            _context.Genres.Add(genre);
            await _context.SaveChangesAsync();
            

            //act
            var result = await _context.ApplicationUsers
                .Include(u => u.FavoriteGenres)
                .FirstOrDefaultAsync(u => u.Id == user.Id);

            //assert
            Assert.NotNull(result);
            Assert.True(await _userGenreService.RemoveGenreFromFavorites(user.Id, genre.Id));
        }
    }
}
