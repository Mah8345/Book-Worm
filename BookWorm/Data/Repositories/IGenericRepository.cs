﻿using System.Linq.Expressions;

namespace BookWorm.Data.Repositories
{
    public interface IGenericRepository<TEntity> where TEntity : class 
    {
        Task<IEnumerable<TEntity>> GetAllAsync();

        Task<IEnumerable<TEntity>> GetAllIncludeAsync<TProperty>(params Expression<Func<TEntity, TProperty>>[] navigationProperties);

        Task<TEntity?> GetByIdAsync(object id);

        Task AddAsync(TEntity entity);

        Task UpdateAsync(TEntity entity);

        Task DeleteAsync(object id);

    }
}
