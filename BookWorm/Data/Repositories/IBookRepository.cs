using BookWorm.Models;

namespace BookWorm.Data.Repositories
{
    public interface IBookRepository
    {
        //gets book with its related entities that are used in book page
        Task<Book?> GetBookByIdAsync(int id);
        
        Task<IEnumerable<Book>> SearchByNameAsync(string key);
    }
}
