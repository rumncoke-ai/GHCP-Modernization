using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace LegacyShop.Data.Repositories
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        IEnumerable<TEntity> GetAll();
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);
        TEntity GetById(int id);
        void Add(TEntity entity);
        void Remove(TEntity entity);
    }
}
