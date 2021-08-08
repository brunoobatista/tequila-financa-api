﻿using System;
using System.Linq;

namespace Tequila.Core
{
    public static class MissingExtensions
    {
        public static PagedResult<T> GetPaged<T>(this IQueryable<T> query, QueryParams parameters) where T : class
        {
            if (parameters.pageSize > 100) parameters.pageSize = 100;
            var result = new PagedResult<T>();
            result.CurrentPage = parameters.page;
            result.PageSize = parameters.pageSize;
            result.Total = query.Count();

            var pageCount = (double) result.Total / parameters.pageSize;
            result.PageCount = (int) Math.Ceiling(pageCount);

            var skip = (parameters.page - 1) * parameters.pageSize;
            result.Results = query.Skip(skip).Take(parameters.pageSize).ToList();
            return result;
        }
    }
}