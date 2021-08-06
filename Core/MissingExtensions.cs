using System;
using System.Linq;

namespace Tequila.Core
{
    public static class MissingExtensions
    {
        public static PagedResult<T> GetPaged<T>(this IQueryable<T> query, int page, int pageSize) where T : class
        {
            if (pageSize > 100) pageSize = 100;
            var result = new PagedResult<T>();
            result.PageSize = pageSize;
            result.RowCount = query.Count();

            var pageCount = (double) result.RowCount / pageSize;
            result.PageCount = (int) Math.Ceiling(pageCount);

            var skip = (page - 1) * pageSize;
            result.Results = query.Skip(skip).Take(pageSize).ToList();
            return result;
        }
    }
}