using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Models;

namespace Repository
{
    public interface IRepository<T> where T : class
    {
        void Delete(T entityToDelete);
        void Delete(int id);
        IEnumerable<T> Get(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null);
        T GetByID(int id);
        IEnumerable<T> GetWithRawSqlAsync(string query,
            params object[] parameters);
        T Insert(T entity);
        void Update(T entityToUpdate);
    }
}
