using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Registration.Core.Repository
{
    public interface IRepository<T> where T : class
    {

        T Find(Expression<Func<T, bool>> match);
        ICollection<T> FindAll(Expression<Func<T, bool>> match);
        Task<ICollection<T>> FindAllAsync(Expression<Func<T, bool>> match);
        Task<T> FindAsync(Expression<Func<T, bool>> match);
        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate);
        Task<ICollection<T>> FindByAsync(Expression<Func<T, bool>> predicate);
        T Get(int id);
        IQueryable<T> GetAll();
        Task<ICollection<T>> GetAllAsync();
        IQueryable<T> GetAllIncluding(params Expression<Func<T, object>>[] includeProperties);
        Task<IQueryable<T>> GetAllIncludingAsync(params Expression<Func<T, object>>[] includeProperties);
        Task<T> GetAsync(int id);
        int Count();
        Task<int> CountAsync();

        T Add(T entity);
        Task<T> AddAsync(T entity);
        T Update(T entity, object key);
        Task<T> UpdateAsync(T entity, object key);
        void Delete(T entity);
        Task<int> DeleteAsync(T entity);
        void Save();
        Task<int> SaveAsync();
        void Dispose();
    }
}
