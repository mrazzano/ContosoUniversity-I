using System;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace ContosoUniversity.Core.Repository
{
    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> Get();
        Task<T> GetByIdAsync(int id);
        IQueryable<T> GetBySearch(Expression<Func<T, bool>> predicate);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }
}
