using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Tequila.Models.Interfaces;
using Tequila.Repositories.Interfaces;

namespace Tequila.Repositories
{
    public abstract class EFCoreRepository<TEntity, TContext> : IRepository<TEntity>
        where TEntity : class, IEntity
        where TContext : DbContext
    {
        private readonly TContext _context;

        public EFCoreRepository(TContext context)
        {
            _context = context;
        }

        public List<TEntity> GetAll()
        {
            var query = _context.Set<TEntity>().AsQueryable();

            return query.Where(e => e.Ativo == 1).AsNoTracking().ToList();
        }
        
        public TEntity Get(long id)
        {
            var query = _context.Set<TEntity>().AsQueryable();

            return _context.Set<TEntity>().AsNoTracking().FirstOrDefault(e => e.Ativo == 1 && e.Id == id);
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
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.Set<TEntity>().Add(entity);
                    _context.SaveChanges();
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
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    entity.AlteradoEm = DateTime.Now;
                    _context.Entry(entity).State = EntityState.Modified;
                    Type type = entity.GetType();
                    PropertyInfo[] properties = type.GetProperties();
                    foreach (var property in properties)
                    {
                        if (property.PropertyType.GetInterfaces().Contains(typeof(IBase)) )
                            continue;

                        if (property.PropertyType.IsGenericType &&
                                property.PropertyType.GetGenericTypeDefinition() == typeof(ICollection<>))
                        {
                            _context.Entry(entity).Collection(property.Name).IsModified = false;
                        }
                        else if (property.GetValue(entity, null) == null)
                        {
                            _context.Entry(entity).Property(property.Name).IsModified = false;
                        }
                    }
                    _context.SaveChanges();
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
            var entity = _context.Set<TEntity>().Find(id);

            if (entity == null) return;

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    entity.Ativo = 0;
                    entity.AlteradoEm = DateTime.Now;
                    _context.Entry(entity).State = EntityState.Modified;
                    _context.SaveChanges();
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
            var entity = _context.Set<TEntity>().Find(id);

            if (entity == null)
            {
                return entity;
            }

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.Set<TEntity>().Remove(entity);
                    _context.SaveChanges();
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