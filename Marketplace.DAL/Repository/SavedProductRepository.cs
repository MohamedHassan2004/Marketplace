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
        public async Task<IEnumerable<SavedProduct>> GetSavedProductsByCustomerIdAsync(string CustomerId)
        {
            return await _dbContext.SavedProducts
                .Where(sp => sp.CustomerId == CustomerId)
                .Include(sp => sp.Product)
                    .ThenInclude(p => p.Category)
                .Include(sp => sp.Product)
                    .ThenInclude(p => p.Vendor)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<bool> SaveProductAsync(SavedProduct savedProduct)
        {
            await _dbContext.SavedProducts.AddAsync(savedProduct);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> UnsaveProductAsync(int Id)
        {
            var productToUnsaved = _dbContext.SavedProducts.FirstOrDefault(sp => sp.Id == Id);
            if (productToUnsaved != null)
            {
                _dbContext.SavedProducts.Remove(productToUnsaved);
                return await _dbContext.SaveChangesAsync() > 0;
            }
            return false;
        }

    }
}
