using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookWorm.Data;
using BookWorm.Data.UnitOfWork;
using BookWorm.Exceptions;
using BookWorm.Models;
using BookWorm.Services;
using BookWorm.Tests.TestHelpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Xunit.Abstractions;

namespace BookWorm.Tests.ServicesTest
{
    public class UserCommentServiceTests
    {
        private ITestOutputHelper _testOutputHelper;
        private readonly UserCommentService _userCommentService;
        private readonly ApplicationDbContext _context;
        public UserCommentServiceTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "BookWormTest")
                .Options;
            var context = new ApplicationDbContext(options);
            _context = context;
            var unitOfWork = new UnitOfWork(context);
            _userCommentService = new UserCommentService(unitOfWork);
        }

        [Fact]
        public async Task AddCommentAsync_CommentIsAdded()
        {
            //arrange
            var user = TestHelper.GenerateRandomUsers(1).First();
            var book = TestHelper.GenerateRandomBooks(1).First();
            var comment = TestHelper.GenerateRandomComments(1).First();
            _context.ApplicationUsers.Add(user);
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
            await _userCommentService.AddCommentAsync(user.Id, book.Id, comment);

            //act
            var result = await _context.Books
                .Include(b => b.Comments)
                .FirstOrDefaultAsync(b => b.Id == book.Id);

            //assert
            Assert.NotNull(result);
            Assert.Contains(result.Comments, c => c.Id == comment.Id);
        }


        [Fact]
        public async Task RemoveCommentAsync_CommentExists_CommentIsRemoved()
        {
            //arrange
            var user = TestHelper.GenerateRandomUsers(1).First();
            var book = TestHelper.GenerateRandomBooks(1).First();
            var comment = TestHelper.GenerateRandomComments(1).First();
            comment.CommentedBy = user;
            book.Comments.Add(comment);
            _context.ApplicationUsers.Add(user);
            _context.Books.Add(book);
            await _context.SaveChangesAsync();


            //act and assert
            Assert.True(await _userCommentService.RemoveCommentAsync(user.Id,book.Id,comment.Id));
        }


        [Fact]
        public async Task RemoveCommentAsync_CommentIsAlreadyRemoved_ThrowsException()
        {
            //arrange
            var user = TestHelper.GenerateRandomUsers(1).First();
            var book = TestHelper.GenerateRandomBooks(1).First();
            var comment = TestHelper.GenerateRandomComments(1).First();
            _context.ApplicationUsers.Add(user);
            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            //act and assert
            var exception = await Assert.ThrowsAsync<EntityNotFoundException>(async () =>
                await _userCommentService.RemoveCommentAsync(user.Id, book.Id, comment.Id));

            Assert.Equal(typeof(Comment), exception.EntityType);
        }
    }
}
