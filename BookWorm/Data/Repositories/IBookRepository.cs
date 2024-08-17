using System.Linq.Expressions;
using BookWorm.Models;

namespace BookWorm.Data.Repositories
{
    public interface IBookRepository : IGenericRepository<Book>
    {
        //gets book with its related entities that are used in book page
        Task<Book?> GetBookByIdAsync(int id);
        

        Task<Book?> GetBookWithAsync<TEntity>(int id , params Expression<Func<Book, TEntity>>[] navigationProperties);


        Task<IEnumerable<Book>> SearchByNameAsync(string key);


        Task<IEnumerable<Book>> GetBooksWithGenreAsync(Genre genre);


        Task<IEnumerable<Book>> GetBooksWithAuthorAsync(Author author);
    }
}
