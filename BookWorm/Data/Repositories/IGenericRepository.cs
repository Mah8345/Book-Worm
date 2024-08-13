﻿namespace BookWorm.Data.Repositories
{
    public interface IGenericRepository<TEntity> where TEntity : class 
    {
        IQueryable<TEntity> GetAllAsync();

        Task<TEntity?> GetByIdAsync(object id);

        Task AddAsync(TEntity entity);

        Task UpdateAsync(TEntity entity);

        Task DeleteAsync(object id);

    }
}
