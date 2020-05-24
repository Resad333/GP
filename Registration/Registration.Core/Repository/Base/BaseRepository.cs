using Microsoft.EntityFrameworkCore;
using Registration.Core.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Registration.Core.Repository
{
    public class BaseRepository<T> : IRepository<T> where T : class
    {

        private readonly CustomerDbContext _context;
        private readonly DbSet<T> _dbSet;

        public BaseRepository(CustomerDbContext context)
        {

            if (context == null)
            {
                throw new ArgumentNullException($"{nameof(CustomerDbContext)} is null");
            }

            this._context = context;
            this._dbSet = context.Set<T>();
        }

        public IQueryable<T> GetAll()
        {
            return _dbSet;
        }
        public virtual async Task<ICollection<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public virtual T Get(int id)
        {
            return _dbSet.Find(id);
        }

        public virtual async Task<T> GetAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual T Find(Expression<Func<T, bool>> match)
        {
            return _dbSet.SingleOrDefault(match);
        }

        public virtual async Task<T> FindAsync(Expression<Func<T, bool>> match)
        {
            return await _dbSet.SingleOrDefaultAsync(match);
        }

        public ICollection<T> FindAll(Expression<Func<T, bool>> match)
        {
            return _dbSet.Where(match).ToList();
        }


        public async Task<ICollection<T>> FindAllAsync(Expression<Func<T, bool>> match)
        {
            return await _dbSet.Where(match).ToListAsync();
        }

        public virtual IQueryable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            IQueryable<T> query = _dbSet.Where(predicate);
            return query;
        }

        public virtual async Task<ICollection<T>> FindByAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public IQueryable<T> GetAllIncluding(params Expression<Func<T, object>>[] includeProperties)
        {

            IQueryable<T> queryable = GetAll();
            foreach (Expression<Func<T, object>> includeProperty in includeProperties)
            {

                queryable = queryable.Include<T, object>(includeProperty);
            }

            return queryable;

        }

        public Task<IQueryable<T>> GetAllIncludingAsync(params Expression<Func<T, object>>[] includeProperties)

        {
            return Task.Run(
                () =>
                {
                    IQueryable<T> queryable = GetAll();
                    foreach (Expression<Func<T, object>> includeProperty in includeProperties)
                    {
                        queryable = queryable.Include<T, object>(includeProperty);
                    }
                    return queryable;

                });
        }

        public virtual T Add(T entity)
        {
            _dbSet.Add(entity);
            _context.SaveChanges();
            return entity;
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            try
            {
                _dbSet.Add(entity);
                await _context.SaveChangesAsync();

            }
            catch (Exception e)
            {

                throw e;
            }

            return entity;

        }
        public virtual T Update(T entity, object key)
        {
            if (entity == null)
            {
                return null;
            }
            T exist = _dbSet.Find(key);
            if (exist != null)
            {
                _context.Entry(exist).CurrentValues.SetValues(entity);
                _context.SaveChanges();
            }
            return exist;
        }

        public virtual async Task<T> UpdateAsync(T entity, object key)
        {
            if (entity == null)
            {
                return null;
            }

            T exist = await _dbSet.FindAsync(key);
            if (exist != null)
            {
                _context.Entry(exist).State = EntityState.Detached;
                _context.Update(entity);
                await _context.SaveChangesAsync();
            }
            return exist;
        }



        public virtual void Delete(T entity)
        {
            _dbSet.Remove(entity);
            _context.SaveChanges();
        }

        public virtual async Task<int> DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            return await _context.SaveChangesAsync();
        }


        public int Count()
        {
            return _dbSet.Count();
        }

        public async Task<int> CountAsync()
        {
            return await _dbSet.CountAsync();
        }

        public virtual void Save()
        {

            _context.SaveChanges();
        }

        public async virtual Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }


        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}
