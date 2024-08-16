using BookWorm.Exceptions;
using BookWorm.Models;
using System.Linq.Expressions;
using BookWorm.Data.UnitOfWork;

namespace BookWorm.Services
{
    public static class EntityServiceUtils
    {
        public static async Task<ApplicationUser> GetUser<TEntity>(string userId,
            Expression<Func<ApplicationUser,TEntity>> navigationProperty, 
            IUnitOfWork unitOfWork)
        {
            var user = await unitOfWork.ApplicationUserRepository.GetUserWith(userId, navigationProperty) 
                       ?? throw new EntityNotFoundException(typeof(ApplicationUser), "user", userId);
            return user;
        }

        public static async Task<Book> GetBook<TEntity>(int bookId,
            Expression<Func<Book,TEntity>> navigationProperty, 
            IUnitOfWork unitOfWork)
        {
            var book = await unitOfWork.BookRepository.GetBookWith(bookId, navigationProperty)
                       ?? throw new EntityNotFoundException(typeof(Book), "book", bookId);
            return book;
        }


        public static async Task<Book> GetBook(int bookId, IUnitOfWork unitOfWork)
        {
            var book = await unitOfWork.BookRepository.GetByIdAsync(bookId)
                       ?? throw new EntityNotFoundException(typeof(Book), "book", bookId);
            return book;
        }


        public static async Task AddToList<T>(T listMember, ICollection<T> list, IUnitOfWork unitOfWork)
        {
            list.Add(listMember);
            await unitOfWork.SaveChangesAsync();
        }


        public static async Task<bool> RemoveFromList<T>(T listMember, ICollection<T> list, IUnitOfWork unitOfWork)
        {
            if (list == null || list.Count == 0)
            {
                throw new ArgumentException("it is not possible to remove a member from a null or empty list");
            }

            var exists = list.Remove(listMember);
            await unitOfWork.SaveChangesAsync();
            return exists;
        }
    }
}
