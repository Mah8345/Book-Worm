using BookWorm.Data.UnitOfWork;

namespace BookWorm.Services
{
    public class UserBookService(IUnitOfWork unitOfWork)
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        
        public async Task AddBookToFavoritesAsync(string userId, int bookId)
        {
            var user = await EntityServiceUtils.GetUser(userId, user => user.FavoriteBooks, _unitOfWork);
            //this happens when the user has no favorite books currently.
            user.FavoriteBooks ??= [];
            var book = await EntityServiceUtils.GetBook(bookId,_unitOfWork);
            await EntityServiceUtils.AddToList(book, user.FavoriteBooks, _unitOfWork);
        }


        public async Task AddBookToReadListAsync(string userId, int bookId)
        {
            var user = await EntityServiceUtils.GetUser(userId, user => user.ReadBooks, _unitOfWork);
            user.ReadBooks ??= [];
            var book = await EntityServiceUtils.GetBook(bookId,_unitOfWork);
            await EntityServiceUtils.AddToList(book, user.ReadBooks, _unitOfWork);
        }


        public async Task AddBookToWantToReadListAsync(string userId, int bookId)
        {
            var user = await EntityServiceUtils.GetUser(userId, user => user.WantToReadBooks, _unitOfWork);
            user.WantToReadBooks ??= [];
            var book = await EntityServiceUtils.GetBook(bookId,_unitOfWork);
            await EntityServiceUtils.AddToList(book, user.WantToReadBooks, _unitOfWork);
        }


        public async Task<bool> RemoveBookFromFavorites(string userId, int bookId)
        {
            var user = await EntityServiceUtils.GetUser(userId, user => user.FavoriteBooks, _unitOfWork);
            var book = await EntityServiceUtils.GetBook(bookId,_unitOfWork);
            var result = await EntityServiceUtils.RemoveFromList(book, user.FavoriteBooks, _unitOfWork);
            return result;
        }


        public async Task<bool> RemoveBookFromWantToReadList(string userId, int bookId)
        {
            var user = await EntityServiceUtils.GetUser(userId, user => user.ReadBooks, _unitOfWork);
            var book = await EntityServiceUtils.GetBook(bookId,_unitOfWork);
            var result = await EntityServiceUtils.RemoveFromList(book, user.ReadBooks, _unitOfWork);
            return result;
        }


        public async Task<bool> RemoveBookFromReadList(string userId, int bookId)
        {
            var user = await EntityServiceUtils.GetUser(userId, user => user.WantToReadBooks, _unitOfWork);
            var book = await EntityServiceUtils.GetBook(bookId,_unitOfWork);
            var result = await EntityServiceUtils.RemoveFromList(book, user.WantToReadBooks, _unitOfWork);
            return result;
        }
    }
}
