using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
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

            return context.Set<TEntity>().FirstOrDefault(e => e.Ativo == 1 && e.Id == id);
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
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    context.Set<TEntity>().Add(entity);
                    context.SaveChanges();
                    transaction.Commit();
                    return entity;
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw new Exception(e.Message);
                }
            }
        }

        public TEntity Update(TEntity entity)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    entity.AlteradoEm = DateTime.Now;
                    context.Entry(entity).State = EntityState.Modified;
                    Type type = entity.GetType();
                    PropertyInfo[] properties = type.GetProperties();
                    foreach (PropertyInfo property in properties)
                    {
                        if (property.PropertyType.GetInterfaces().Contains(typeof(IBase)) )
                            continue;
                        
                        if (property.GetValue(entity, null) == null)
                        {
                            context.Entry(entity).Property(property.Name).IsModified = false;
                        }
                    }
                    context.SaveChanges();
                    transaction.Commit();
                    return entity;
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw new Exception(e.Message);
                }
            }
        }
        
        public void Inactive(long id)
        {
            var entity = context.Set<TEntity>().Find(id);

            if (entity == null) return;

            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    entity.Ativo = 0;
                    entity.AlteradoEm = DateTime.Now;
                    context.Entry(entity).State = EntityState.Modified;
                    context.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw new Exception(e.Message);
                }
            }
        }

        public TEntity Delete(long id)
        {
            var entity = context.Set<TEntity>().Find(id);

            if (entity == null)
            {
                return entity;
            }

            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    context.Set<TEntity>().Remove(entity);
                    context.SaveChanges();
                    transaction.Commit();
                    return entity;
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw new Exception(e.Message);
                }
            }
        }
    }
}