using System.Linq.Expressions;

namespace Marketplace.DAL.IRepository
{
    public interface IRepo<T> where T : class
    {
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<bool> AddAsync(T entity);
        Task<bool> UpdateAsync(T entity);
        Task<bool> DeleteByIdAsync(int Id);
        Task<IEnumerable<T>> FilterAsync(Expression<Func<T, bool>> predicate);
    }
}
