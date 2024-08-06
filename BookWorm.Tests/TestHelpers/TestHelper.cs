using BookWorm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookWorm.Data;

namespace BookWorm.Tests.TestHelpers
{
    public static class TestHelper
    {
        public static List<Book> GenerateRandomBooks(int count)
        {
            var books = new List<Book>();
            for (int i = 0; i < count; i++)
            {
                books.Add(new Book()
                {
                    Title = $"Book_{i}",
                    NormalizedTitle = $"BOOK_{i}",
                    Introduction = $"introduction_{i}",
                    Summary = $"summary_{i}",
                    PagesNumber = 100 + (i % 3) * 20
                });
            }

            return books;
        }

        public static List<Genre> GenerateRandomGenres(int count)
        {
            var genres = new List<Genre>();
            for (int i = 0; i < count; i++)
            {
                genres.Add(new Genre()
                {
                    Name = $"Genre_{i}",
                    NormalizedName = $"GENRE_{i}",
                    Description = $"description_{i}"
                });
            }

            return genres;
        }

        public static List<Author> GenerateRandomAuthors(int count)
        {
            var authors = new List<Author>();
            for (int i = 0; i < count; i++)
            {
                authors.Add(new Author()
                {
                    Name = $"Author{i}",
                    NormalizedName = $"AUTHOR{i}",
                    About = $"about Author_{i}"
                });
            }

            return authors;
        }

        public static ApplicationUser AddUser(string id,ApplicationDbContext context)
        {
            var user = new ApplicationUser()
            {
                Id = id,
                FirstName = "FirstName",
                LastName = "LastName",
                AccessFailedCount = 10,
                Bio = "Hey, I'm very interested in reading!",
                Comments = new List<Comment>(),
                DateOfBirth = DateTime.Now,
                Email = "example@ex.com",
                EmailConfirmed = false,
                FavoriteAuthors = GenerateRandomAuthors(10),
                FavoriteBooks = GenerateRandomBooks(10),
                FavoriteGenres = GenerateRandomGenres(10)
            };
            context.ApplicationUsers.Add(user);
            context.SaveChanges();
            return user;
        }
    }
}
