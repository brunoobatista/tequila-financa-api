using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Tequila.Models.Interfaces;

namespace Tequila.Repositories.Interfaces
{
    public interface IRepository<T> where T : class, IEntity
    {
        List<T> GetAll();
        //T Get(Expression<Func<T, bool>> predicate, params string[] navigationProperties);
        T Get(long id);
        T Add(T entity);
        T Update(T entity);
        T Delete(long id);
    }
}