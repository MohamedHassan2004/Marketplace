﻿using Marketplace.DAL.Context;
using Marketplace.DAL.IRepository;
using Marketplace.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Marketplace.DAL.Repository
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

        public async Task<bool> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteByIdAsync(int Id)
        {
            var product = await _context.Products.FindAsync(Id);
            if (product == null) return false;

            _context.Products.Remove(product);
            return await _context.SaveChangesAsync() > 0;
        }


        public Task<IEnumerable<T>> FilterAsync(Expression<Func<T, bool>> predicate)
        {
            var query = _dbSet.AsQueryable().Where(predicate);
            return Task.FromResult<IEnumerable<T>>(query.ToList());
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<bool> UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
