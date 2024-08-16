using BookWorm.Data.UnitOfWork;

namespace BookWorm.Services
{
    public class UserAuthorService(IUnitOfWork unitOfWork)
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;


        public async Task AddAuthorToFavoritesAsync(string userId, int authorId)
        {
            var user = await EntityServiceUtils.GetUser(userId, user => user.FavoriteAuthors, unitOfWork);
            var author = await EntityServiceUtils.GetAuthor(authorId, unitOfWork);
            await EntityServiceUtils.AddToList(author, user.FavoriteAuthors, unitOfWork);
        }


        public async Task<bool> RemoveAuthorFromFavorites(string userId, int authorId)
        {
            var user = await EntityServiceUtils.GetUser(userId, user => user.FavoriteAuthors, unitOfWork);
            var author = await EntityServiceUtils.GetAuthor(authorId, unitOfWork);
            var result = await EntityServiceUtils.RemoveFromList(author, user.FavoriteAuthors, unitOfWork);
            return result;
        }
    }
}
