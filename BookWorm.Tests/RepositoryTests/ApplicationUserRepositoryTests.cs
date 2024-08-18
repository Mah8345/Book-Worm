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
    public class ApplicationUserRepositoryTests
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly ApplicationDbContext _context;
        private readonly ApplicationUserRepository _userRepository;

        public ApplicationUserRepositoryTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: $"BookWormTest_{Guid.NewGuid()}")
                .LogTo(Console.WriteLine,LogLevel.Information)
                .Options;
            _context = new ApplicationDbContext(options);
            _userRepository = new ApplicationUserRepository(_context);
        }

        
        
        //method name_state under test_expected behavior
        [Fact]
        public async Task GetUserByIdAsync_UserExists_ReturnsUser()
        {
            //Arrange
            var id = Guid.NewGuid().ToString();
            var user = TestHelper.AddUser(id,_context);

            //Act
            var result = await _userRepository.GetUserByIdAsync(id);


            //Assert
            Assert.NotNull(result);
            Assert.Equal(id, result.Id);
            Assert.Equal(user.Bio,result.Bio);

            Assert.NotEmpty(result.FavoriteBooks);
            for (int i = 1; i < 11; i++)
            {
                Assert.Contains(result.FavoriteBooks, a => a.Id == i);
            }

            Assert.NotEmpty(result.FavoriteAuthors);
            for (int i = 1; i < 11; i++)
            {
                Assert.Contains(result.FavoriteAuthors, a => a.Id == i);
            }

            Assert.NotEmpty(result.FavoriteGenres);
            for (int i = 1; i < 11; i++)
            {
                Assert.Contains(result.FavoriteGenres, g => g.Id == i);
            }
        }

        [Fact]
        public async Task GetUserByIdAsync_UserDoesNotExist_ReturnsNull()
        {
            //Arrange
            var id = Guid.NewGuid().ToString();

            //Act
            var result = await _userRepository.GetUserByIdAsync(id);

            //Assert
            Assert.Null(result);
        }
    }
}
