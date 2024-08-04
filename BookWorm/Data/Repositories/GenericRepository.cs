using BookWorm.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace BookWorm.Data.Repositories
{
    public class GenericRepository<TEntity>(ApplicationDbContext context) : IGenericRepository<TEntity> where TEntity : class
    {
        protected readonly ApplicationDbContext Context = context;
        protected readonly DbSet<TEntity> DbSet = context.Set<TEntity>();


        public IQueryable<TEntity> GetAllAsync()
        {
            return DbSet;
        }

        public async Task<TEntity?> GetByIdAsync(object id)
        {
            return await DbSet.FindAsync(id);
        }

        public async Task AddAsync(TEntity entity)
        {
            await DbSet.AddAsync(entity);
        }

        public async Task UpdateAsync(TEntity entity)
        {
            DbSet.Update(entity);
        }

        public async Task DeleteAsync(object id)
        {
            var entity = await DbSet.FindAsync(id);
            if (entity == null) throw new EntityNotFoundException(nameof(entity), id);

            DbSet.Remove(entity);
        }

        public async Task SaveAsync()
        {
            await Context.SaveChangesAsync();
        }
    }
}
