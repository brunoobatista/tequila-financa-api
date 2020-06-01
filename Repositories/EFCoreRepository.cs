using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Tequila.Models.Interfaces;
using Tequila.Repositories.Interfaces;

namespace Tequila.Repositories
{
    public abstract class EFCoreRepository<TEntity, TContext> : IRepository<TEntity>
        where TEntity : class, IEntity
        where TContext : DbContext
    {
        private readonly TContext context;

        public EFCoreRepository(TContext context)
        {
            this.context = context;
        }

        public List<TEntity> GetAll()
        {
            var query = this.context.Set<TEntity>().AsQueryable();

            return query.Where(e => e.Ativo == 1).AsNoTracking().ToList();
        }
        
        public TEntity Get(long id)
        {
            var query = this.context.Set<TEntity>().AsQueryable();

            return this.context.Set<TEntity>().SingleOrDefault(e => e.Ativo == 1 && e.Id == id);
        }

        // public TEntity Get(Expression<Func<TEntity, bool>> predicate, params string[] navigationProperties)
        // {
        //     var query = this.context.Set<TEntity>().AsQueryable();
        //
        //     foreach (string navigation in navigationProperties)
        //         query.Include(navigation);
        //
        //     return query.Where(predicate).AsNoTracking().SingleOrDefault();
        // }

        public TEntity Add(TEntity entity)
        {
            this.context.Set<TEntity>().Add(entity);
            this.context.SaveChanges();
            return entity;
        }

        public TEntity Update(TEntity entity)
        {
            this.context.Entry(entity).State = EntityState.Modified;
            this.context.SaveChanges();
            return entity;
        }

        public TEntity Delete(long id)
        {
            var entity = this.context.Set<TEntity>().Find(id);

            if (entity == null)
            {
                return entity;
            }

            this.context.Set<TEntity>().Remove(entity);
            this.context.SaveChanges();
            return entity;
        }
    }
}