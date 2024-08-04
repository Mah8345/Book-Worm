using BookWorm.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookWorm.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<ApplicationImage> ApplicationImages { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Award> Awards { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            ConfigureApplicationUser(builder);
            ConfigureBook(builder);
            
        }

        private void ConfigureApplicationUser(ModelBuilder builder)
        {
            builder.Entity<ApplicationUser>()
                .HasMany<Book>(u => u.FavoriteBooks)
                .WithMany()
                .UsingEntity<Dictionary<string, object>>
                (
                    "FavoriteBooks",
                    r => r.HasOne<Book>().WithMany().HasForeignKey("BookId"),
                    l => l.HasOne<ApplicationUser>().WithMany().HasForeignKey("UserId")
                );
            builder.Entity<ApplicationUser>()
                .HasMany<Book>(u => u.ReadBooks)
                .WithMany()
                .UsingEntity<Dictionary<string, object>>
                (
                    "ReadBooks",
                    r => r.HasOne<Book>().WithMany().HasForeignKey("BookId"),
                    l => l.HasOne<ApplicationUser>().WithMany().HasForeignKey("UserId")
                );
            builder.Entity<ApplicationUser>()
                .HasMany<Book>(u => u.WantToReadBooks)
                .WithMany()
                .UsingEntity<Dictionary<string, object>>
                (
                    "WantToReadBooks",
                    r => r.HasOne<Book>().WithMany().HasForeignKey("BookId"),
                    l => l.HasOne<ApplicationUser>().WithMany().HasForeignKey("UserId")
                );
            builder.Entity<ApplicationUser>()
                .HasMany<Author>(u => u.FavoriteAuthors)
                .WithMany()
                .UsingEntity<Dictionary<string, object>>
                (
                    "FavoriteAuthors",
                    r => r.HasOne<Author>().WithMany().HasForeignKey("AuthorId"),
                    l => l.HasOne<ApplicationUser>().WithMany().HasForeignKey("UserId")
                );
            builder.Entity<ApplicationUser>()
                .HasMany<Genre>(u => u.FavoriteGenres)
                .WithMany()
                .UsingEntity<Dictionary<string, object>>
                (
                    "FavoriteGenres",
                    r => r.HasOne<Genre>().WithMany().HasForeignKey("GenreId"),
                    l => l.HasOne<ApplicationUser>().WithMany().HasForeignKey("UserId")
                );
        }

        private void ConfigureBook(ModelBuilder builder)
        {
            builder.Entity<Book>()
                .HasMany<Author>(b => b.Authors)
                .WithMany(a => a.WrittenBooks)
                .UsingEntity<Dictionary<string, object>>
                (
                    "AuthorBooks",
                    r => r.HasOne<Author>().WithMany().HasForeignKey("AuthorId"),
                    l => l.HasOne<Book>().WithMany().HasForeignKey("BookId")
                );

            builder.Entity<Book>()
                .HasMany<Award>(b => b.Awards)
                .WithMany(a => a.AwardedBooks)
                .UsingEntity<Dictionary<string, object>>
                (
                    "AwardBooks",
                    r => r.HasOne<Award>().WithMany().HasForeignKey("AwardId"),
                    l => l.HasOne<Book>().WithMany().HasForeignKey("BookId")
                );

            builder.Entity<Book>()
                .HasMany<Review>(b => b.Reviews)
                .WithOne()
                .HasForeignKey("BookId");

            builder.Entity<Book>()
                .HasMany<Comment>(b => b.Comments)
                .WithOne()
                .HasForeignKey("BookId");

            builder.Entity<Book>()
                .HasMany<Genre>(b => b.AssociatedGenres)
                .WithMany(g => g.AssociatedBooks)
                .UsingEntity<Dictionary<string, object>>
                (
                    "GenreBooks",
                    r => r.HasOne<Genre>().WithMany().HasForeignKey("GenreId"),
                    l => l.HasOne<Book>().WithMany().HasForeignKey("BookId")
                );

            builder.Entity<Book>()
                .HasMany<Book>(b => b.SimilarBooks)
                .WithMany()
                .UsingEntity<Dictionary<string, object>>
                    (
                        "SimilarBooks",
                        r => r.HasOne<Book>().WithMany().HasForeignKey("SimilarBookId"),
                        l=>l.HasOne<Book>().WithMany().HasForeignKey("BookId")
                    );
        }
    }
}
