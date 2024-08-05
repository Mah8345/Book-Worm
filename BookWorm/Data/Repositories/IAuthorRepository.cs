using BookWorm.Models;

namespace BookWorm.Data.Repositories
{
    public interface IAuthorRepository
    {
        Task<Author?> GetAuthorWithPhotoAsync(int id);
        Task<Author?> GetAuthorWithPhotoAndBooksAsync(int id);
        Task<IEnumerable<Author>> SearchAuthorsByNameAsync(string key);
    }
}