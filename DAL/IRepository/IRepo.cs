using System.Linq.Expressions;

namespace Marketplace.DAL.IRepository
{
    public interface IRepo<T> where T : class
    {
        Task<T> GetById(int id);
        Task<IEnumerable<T>> GetAll();
        Task<bool> Add(T entity);
        Task<bool> Update(T entity);
        Task<bool> Delete(int id);
        Task<IEnumerable<T>> Filter(Expression<Func<T, bool>> predicate);
    }
}
