using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Repository.EF
{
    public class EFRepository<T> : IRepository<T> where T : class
    {
        private ChallengerContext _context;
        private DbSet<T> _dbSet;

        public EFRepository(ChallengerContext context)
        {
            this._context = context;
            this._dbSet = context.Set<T>();
        }

        public virtual IEnumerable<T> GetWithRawSqlAsync(string query,
            params object[] parameters)
        {
            return _dbSet.FromSqlRaw<T>(query, parameters).ToListAsync().Result;
        }

        public virtual IEnumerable<T> GetAsync(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null)
        {
            using (var challengerContext = new ChallengerContext())
            {
                IQueryable<T> query = challengerContext.Set<T>();

                if (filter != null)
                {
                    query = query.Where(filter);
                }
                if (orderBy != null)
                {
                    return orderBy(query).ToList();
                }
                else
                {
                    return query.ToList();
                }
            }
        }

        public virtual T GetByID(int id)
        {
            return _dbSet.Find(id);
        }

        public virtual T Insert(T entity)
        {
            var entry = _dbSet.Add(entity);
                _context.SaveChanges();
            return entry.Entity;
        }

        public virtual void Delete(int id)
        {
            T entityToDelete = _dbSet.Find(id);
            Delete(entityToDelete);
        }

        public virtual void Delete(T entityToDelete)
        {
            if (_context.Entry(entityToDelete).State == EntityState.Detached)
            {
                _dbSet.Attach(entityToDelete);
            }
            _dbSet.Remove(entityToDelete);
        }

        public virtual void Update(T entityToUpdate)
        {
            _dbSet.Add(entityToUpdate);
            _context.Entry(entityToUpdate).State = EntityState.Modified;
            _context.SaveChanges();
        }
    }
}
