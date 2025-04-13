using Marketplace.DAL.Context;
using Marketplace.DAL.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Socialify.DAL.Repository
{
    public class Repo<T> : IRepo<T> where T : class
    {
        private readonly MarketplaceDbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repo(MarketplaceDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<bool> Add(T entity)
        {
            await _dbSet.AddAsync(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> Delete(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null)
                return false;

            _dbSet.Remove(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public Task<IEnumerable<T>> Filter(Expression<Func<T, bool>> predicate)
        {
            var query = _dbSet.AsQueryable().Where(predicate);
            return Task.FromResult<IEnumerable<T>>(query.ToList());
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T> GetById(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<bool> Update(T entity)
        {
            _dbSet.Update(entity);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
