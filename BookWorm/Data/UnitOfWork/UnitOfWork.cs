using BookWorm.Data.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace BookWorm.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        public ApplicationDbContext Context { get; }
        public ApplicationUserRepository ApplicationUserRepository { get; }
        public AuthorRepository AuthorRepository { get; }
        public AwardRepository AwardRepository { get; }
        public BookRepository BookRepository { get; }
        public GenreRepository GenreRepository { get; }
        public PublisherRepository PublisherRepository { get; }
        public ReviewRepository ReviewRepository { get; }

        private IDbContextTransaction? _transactionObj;


        public UnitOfWork(ApplicationDbContext context)
        {
            Context = context;
            ApplicationUserRepository = new ApplicationUserRepository(context);
            AuthorRepository = new AuthorRepository(context);
            AwardRepository = new AwardRepository(context);
            BookRepository = new BookRepository(context);
            GenreRepository = new GenreRepository(context);
            PublisherRepository = new PublisherRepository(context);
            ReviewRepository = new ReviewRepository(context);
        }

        public void BeginTransaction()
        {
            _transactionObj = Context.Database.BeginTransaction();
        }


        public void Commit()
        {
            try
            {
                Context.SaveChanges();
                _transactionObj?.Commit();
            }
            catch
            {
                Rollback();
                throw;
            }
        }

        public void Rollback()
        {
            _transactionObj?.Rollback();
            _transactionObj?.Dispose();
        }

        public void Dispose()
        {
            _transactionObj?.Dispose();
            Context.Dispose();
        }

    }
}
