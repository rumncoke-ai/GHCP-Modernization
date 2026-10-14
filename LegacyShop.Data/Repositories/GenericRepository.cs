using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace LegacyShop.Data.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        protected readonly StoreDbContext Context;
        protected readonly DbSet<TEntity> DbSet;

        public GenericRepository(StoreDbContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            Context = context;
            DbSet = context.Set<TEntity>();
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            return DbSet.ToList();
        }

        public virtual IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.Where(predicate).ToList();
        }

        public virtual TEntity GetById(int id)
        {
            return DbSet.Find(id);
        }

        public virtual void Add(TEntity entity)
        {
            DbSet.Add(entity);
        }

        public virtual void Remove(TEntity entity)
        {
            DbSet.Remove(entity);
        }
    }
}
