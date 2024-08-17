using BookWorm.Data.UnitOfWork;

namespace BookWorm.Services
{
    public class UserGenreService(IUnitOfWork unitOfWork)
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;


        public async Task AddGenreToFavoritesAsync(string userId, int genreId)
        {
            var user = await EntityServiceUtils.GetUser(userId, user => user.FavoriteGenres, _unitOfWork);
            var genre = await EntityServiceUtils.GetGenre(genreId, _unitOfWork);
            await EntityServiceUtils.AddToList(genre, user.FavoriteGenres, _unitOfWork);
        }


        public async Task<bool> RemoveGenreFromFavorites(string userId, int genreId)
        {
            var user = await EntityServiceUtils.GetUser(userId, user => user.FavoriteGenres, _unitOfWork);
            var genre = await EntityServiceUtils.GetGenre(genreId, _unitOfWork);
            var result = await EntityServiceUtils.RemoveFromList(genre, user.FavoriteGenres, _unitOfWork);
            return result;
        }
    }
}
