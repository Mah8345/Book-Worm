using System.Linq.Expressions;
using BookWorm.Models;

namespace BookWorm.Data.Repositories
{
    public interface IBookRepository : IGenericRepository<Book>
    {
        //gets book with its related entities that are used in book page
        Task<Book?> GetBookByIdAsync(int id);
        

        Task<Book?> GetBookWith<TEntity>(int id , params Expression<Func<Book, TEntity>>[] navigationProperties);


        Task<IEnumerable<Book>> SearchByNameAsync(string key);


        Task<IEnumerable<Book>> GetBooksWithGenre(Genre genre);


        Task<IEnumerable<Book>> GetBooksWithAuthor(Author author);
    }
}
