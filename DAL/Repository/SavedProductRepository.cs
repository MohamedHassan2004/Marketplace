using Marketplace.DAL.Context;
using Marketplace.DAL.IRepository;
using Marketplace.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.DAL.Repository
{
    public class SavedProductRepository : ISavedProductRepository
    {
        private readonly MarketplaceDbContext _dbContext;
        public SavedProductRepository(MarketplaceDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IEnumerable<Product>> GetSavedProductsByCustomerIdAsync(string CustomerId)
        {
            return await _dbContext.SavedProducts
                .Where(sp => sp.CustomerId == CustomerId)
                    .Select(sp => sp.Product)
                    .Include(p => p.Reviews)
                    .Include(p => p.Category)
                    .Include(p => p.Vendor)
                    .ToListAsync();
        }

        public async Task<bool> SaveProductAsync(SavedProduct savedProduct)
        {
            await _dbContext.SavedProducts.AddAsync(savedProduct);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> UnsaveProductAsync(SavedProduct savedProduct)
        {
            _dbContext.SavedProducts.Remove(savedProduct);
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}
