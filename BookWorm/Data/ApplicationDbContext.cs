using BookWorm.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookWorm.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : IdentityDbContext<ApplicationUser>(options)
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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);
            optionsBuilder.UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            ConfigureApplicationImage(builder);
            ConfigureApplicationUser(builder);
            ConfigureBook(builder);
            ConfigureAuthor(builder);
            ConfigureGenre(builder);
            ConfigurePublisher(builder);
            ConfigureComment(builder);
        }

        private void ConfigureApplicationImage(ModelBuilder builder)
        {
            builder.Entity<ApplicationImage>()
                .Property(i => i.UploadedOn)
                .HasColumnName("UploadedOn");
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
                .HasIndex(b => b.NormalizedTitle);

            builder.Entity<Book>()
                .Property(b => b.NormalizedTitle)
                .HasComputedColumnSql("UPPER(Title)");

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
                .WithOne(r => r.ReviewedBook)
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

        private void ConfigureAuthor(ModelBuilder builder)
        {
            builder.Entity<Author>()
                .HasIndex(a => a.NormalizedName);

            builder.Entity<Author>()
                .Property(a => a.NormalizedName)
                .HasComputedColumnSql("UPPER(Name)");
        }

        private void ConfigureGenre(ModelBuilder builder)
        {
            builder.Entity<Genre>()
                .HasIndex(g => g.NormalizedName);

            builder.Entity<Genre>()
                .Property(g => g.NormalizedName)
                .HasComputedColumnSql("UPPER(Name)");
        }

        private void ConfigurePublisher(ModelBuilder builder)
        {
            builder.Entity<Publisher>()
                .HasIndex(p => p.NormalizedName);

            builder.Entity<Publisher>()
                .Property(p => p.NormalizedName)
                .HasComputedColumnSql("UPPER(Name)");
        }

        private void ConfigureComment(ModelBuilder builder)
        {
            builder.Entity<Comment>()
                .Property(c => c.CommentedAt)
                .HasColumnName("CommentedAt");
        }
    }
}
