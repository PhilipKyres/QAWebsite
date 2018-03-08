using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace QAWebsite.Extensions
{
    public static class DbSetExtensions
    {
        public static T AddIfNotExists<T>(this DbSet<T> dbSet, T entity, Expression<Func<T, bool>> predicate = null) where T : class, new()
        {
            T item = predicate == null ? dbSet.FirstOrDefault() : dbSet.FirstOrDefault(predicate);
            return item ?? dbSet.Add(entity).Entity;
        }
    }
}
