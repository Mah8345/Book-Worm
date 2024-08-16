using BookWorm.Data.UnitOfWork;
using BookWorm.Models;

namespace BookWorm.Services
{
    public class UserCommentService(IUnitOfWork unitOfWork)
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task AddComment(string userId, int bookId, Comment comment)
        {
            var user = await EntityServiceUtils.GetUser(userId, user => user.Comments, unitOfWork);
            var book = await EntityServiceUtils.GetBook(bookId, book => book.Comments, unitOfWork);
            comment.CommentedBy = user;
            book.Comments ??= [];
            await _unitOfWork.SaveChangesAsync();
            await EntityServiceUtils.AddToList(comment, book.Comments, unitOfWork);
        }


        public async Task<bool> RemoveComment(string userId, int bookId, int commentId)
        {
            var user = await EntityServiceUtils.GetUser(userId, user => user.Comments, unitOfWork);
            var book = await EntityServiceUtils.GetBook(bookId, book => book.Comments, unitOfWork);
            var comment = await EntityServiceUtils.GetComment(commentId, unitOfWork);
            comment.CommentedBy = user;
            var result = await EntityServiceUtils.RemoveFromList(comment, book.Comments, _unitOfWork);
            return result;
        }
    }
}
