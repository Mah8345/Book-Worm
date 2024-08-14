using BookWorm.Data.Repositories;

namespace BookWorm.Data.UnitOfWork
{
    public interface IUnitOfWork
    { 
        ApplicationDbContext Context { get; }
        ApplicationUserRepository ApplicationUserRepository { get; }
        AuthorRepository AuthorRepository { get; }
        AwardRepository AwardRepository { get; }
        BookRepository BookRepository { get; }
        GenreRepository GenreRepository { get; }
        PublisherRepository PublisherRepository { get; }
        ReviewRepository ReviewRepository { get; }
        Task<int> SaveChangesAsync();
        void BeginTransaction();
        Task CommitAsync();
        void Rollback();
    }
}
