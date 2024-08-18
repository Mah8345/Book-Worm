using BookWorm.Data;
using BookWorm.Data.Repositories;
using BookWorm.Models;

namespace BookWorm.Services
{
    public class BookRecommender(IBookRepository bookRepository)
    {
        public async Task<List<Book>> RecommendBasedOnFavoriteGenresAsync(List<Genre> favoriteGenres, int count)
        {
            var recommendedBooks = new List<Book>();
            foreach (var genre in favoriteGenres)
            {
                var books = await bookRepository.GetBooksWithGenreAsync(genre);
                recommendedBooks.AddRange(books);
            }
            return recommendedBooks.Take(count).ToList();
        }


        public async Task<List<Book>> RecommendBasedOnFavoriteAuthorsAsync(List<Author> favoriteAuthors, int count)
        {
            var recommendedBooks = new List<Book>();
            foreach (var author in favoriteAuthors)
            {
                var books = await bookRepository.GetBooksWithAuthorAsync(author);
                recommendedBooks.AddRange(books);
            }
            return recommendedBooks.Take(count).ToList();
        }


        public async Task<List<Book>> RecommendBasedOnRatingAsync(int count)
        {
            var books = await bookRepository.GetAllAsync();
            books = books.OrderByDescending(b => b.AverageRating).Take(count);
            return books.ToList();
        }


        public async Task<List<Book>> RecommendBasedOnPopularityAsync(int count)
        {
            var books = await bookRepository.GetAllBooksIncludeAsync(b => b.FavoritedByUsers);
            books = books.OrderByDescending(b => b.FavoritedByUsers.Count).Take(count);
            return books.ToList();
        }


        public async Task<List<Book>> RecommendBasedOnIntroductionDate(int count)
        {
            var books = await bookRepository.GetAllBooksIncludeAsync(b => b.IntroducedAt);
            books = books.OrderByDescending(b => b.IntroducedAt).Take(count);
            return books.ToList();
        }
    }
}
