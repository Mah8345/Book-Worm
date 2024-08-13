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
        public static List<ApplicationImage> GenerateRandomImages(int count)
        {
            var images = new List<ApplicationImage>();
            for (int i = 0; i < count; i++)
            {
                images.Add(new ApplicationImage()
                {
                    FileName = $"Name_{i}",
                    FileSize = 1024 + i
                });
            }

            return images;
        }

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


        public static List<Genre> GenerateRandomGenres(int count, bool generateImageForGenres = false)
        {
            List<ApplicationImage>? images = null;
            if (generateImageForGenres)
            {
                images = GenerateRandomImages(count);
            }
            var genres = new List<Genre>();
            for (int i = 0; i < count; i++)
            {
                genres.Add(new Genre()
                {
                    Name = $"Genre_{i}",
                    NormalizedName = $"GENRE_{i}",
                    Description = $"description_{i}",
                    Photo = images?[i]
                });
            }

            return genres;
        }


        public static List<Author> GenerateRandomAuthors(int count, bool generateImageForAuthors = false)
        {
            List<ApplicationImage>? images = null;
            if (generateImageForAuthors)
            {
                images = GenerateRandomImages(count);
            }
            var authors = new List<Author>();
            for (int i = 0; i < count; i++)
            {
                authors.Add(new Author()
                {
                    Name = $"Author{i}",
                    NormalizedName = $"AUTHOR{i}",
                    About = $"about Author_{i}",
                    Photo = images?[i]
                });
            }

            return authors;
        }

        public static List<Publisher> GenerateRandomPublishers(int count, bool generateImageForPublishers = false)
        {
            List<ApplicationImage>? images = null;
            if (generateImageForPublishers)
            {
                images = GenerateRandomImages(count);
            }
            var publishers = new List<Publisher>();
            for (var i = 0; i < count; i++)
            {
                publishers.Add(new Publisher()
                {
                    Name = $"Publisher_{i}",
                    NormalizedName = $"PUBLISHER_{i}",
                    About = $"about Publisher_{i}",
                    PublisherLogo = images?[i]
                });
            }

            return publishers;
        }

        public static List<Award> GenerateRandomAwards(int count)
        {
            var awards = new List<Award>();
            for (int i = 0; i < count; i++)
            {
                awards.Add(new Award()
                {
                    Name = $"Award_{i}",
                    About = $"About Award_{i}"
                });
            }

            return awards;
        }

        public static List<Comment> GenerateRandomComments(int count)
        {
            var users = GenerateRandomUsers(count);
            var comments = new List<Comment>();
            for (int i = 0; i < count; i++)
            {
                comments.Add(new Comment()
                {
                    CommentedAt = DateTime.Now,
                    Rating = 5,
                    Body = $"Comment_{i}",
                    CommentedBy = users[i]
                });
            }

            return comments;
        }

        public static List<Review> GenerateRandomReviews(int count)
        {
            var users = GenerateRandomUsers(count);
            var reviews = new List<Review>();
            for (int i = 0; i < count; i++)
            {
                reviews.Add(new Review()
                {
                    Title = $"Review_{i}",
                    Body = $"Review Body_{i}",
                    ReviewedBy = users[i]
                });
            }

            return reviews;
        }

        public static List<ApplicationUser> GenerateRandomUsers(int count)
        {
            var random = new Random();
            var age = random.Next(12, 80);
            var users = new List<ApplicationUser>();
            for (int i = 0; i < count; i++)
            {
                users.Add(new ApplicationUser()
                {
                    FirstName = $"FirstName_{i}",
                    LastName = $"LastName_{i}",
                    AccessFailedCount = 10,
                    Bio = $"Hey, I'm very interested in reading!_{i}",
                    DateOfBirth = DateTime.Now.AddYears(-age),
                    Email = $"ex_{i}@example.com"
                });
            }
            return users;
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

        public static void SeedDatabase(ApplicationDbContext context)
        {
            var users = GenerateRandomUsers(20);
            var books = GenerateRandomBooks(10);
            var authors = GenerateRandomAuthors(10);
            var genres = GenerateRandomGenres(10);
            context.ApplicationUsers.AddRange(users);
            foreach (var user in users)
            {
                user.FavoriteBooks = books;
                user.FavoriteAuthors = authors;
                user.FavoriteGenres = genres;
            }

            context.SaveChanges();
        }
    }
}
