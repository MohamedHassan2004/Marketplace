using Marketplace.DAL.Context;
using Marketplace.DAL.Enums;
using Marketplace.DAL.IRepository;
using Marketplace.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.DAL.Repository
{
    public class ProductRepository : Repo<Product>, IProductRepository
    {
        private readonly MarketplaceDbContext _dbContext;

        public ProductRepository(MarketplaceDbContext context) : base(context)
        {
            _dbContext = context;
        }

        private IQueryable<Product> GetBaseProductQuery()
        {
            return _dbContext.Products
                .Include(p => p.Category)
                .Include(p => p.Vendor)
                .Include(p => p.AdminChecked)
                .AsNoTracking();
        }
        private IQueryable<Product> GetAcceptedProductQuery()
        {
            return _dbContext.Products
                .Where(p => p.ApprovalStatus == ApprovalStatus.Approved)
                .Include(p => p.Category)
                .Include(p => p.Vendor)
                .Include(p => p.AdminChecked)
                .Include(p => p.SavedProducts)
                .Include(p => p.OrderItems)
                    .ThenInclude(oi => oi.Order)
                .AsNoTracking()
;
        }


        // customer , admin , vendor
        public async Task<IEnumerable<Product>> GetAcceptedProductsAsync()
        {
            return await GetAcceptedProductQuery().ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsByProductNameOrCategoryNameAsync(string Name)
        {
            var name = Name.ToLower();
            return await GetAcceptedProductQuery()
                .Where(p => p.Title.ToLower().Contains(name) || p.Category.Name.ToLower().Contains(name))
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetAcceptedProductsByVendorIdAsync(string vendorId)
        {
            return await GetAcceptedProductQuery()
                .Where(p => p.VendorId == vendorId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsByPriceRangeAsync(decimal minPrice, decimal maxPrice)
        {
            return await GetAcceptedProductQuery()
                .Where(p => p.Price >= minPrice && p.Price <= maxPrice)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryIdAsync(int categoryId)
        {
            return await GetAcceptedProductQuery()
                .Where(p => p.Category.Id == categoryId)
                .ToListAsync();
        }


        // admin
        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await GetBaseProductQuery().ToListAsync();
        }
        public async Task<IEnumerable<Product>> GetRejectedProductsAsync()
        {
            return await GetBaseProductQuery()
                .Where(p => p.ApprovalStatus == ApprovalStatus.Rejected)
                .ToListAsync();
        }
        public async Task<IEnumerable<Product>> GetWaitingProductsAsync()
        {
            return await GetBaseProductQuery()
                .Where(p => p.ApprovalStatus == ApprovalStatus.Pending)
                .ToListAsync();
        }
        public async Task<Product> GetProductByIdAsync(int productId)
        {
            return await GetBaseProductQuery()
                .FirstOrDefaultAsync(p => p.Id == productId);
        }

        // vendor , admin
        public async Task<IEnumerable<Product>> GetAllProductsByVendorIdAsync(string vendorId)
        {
            return await GetBaseProductQuery()
                .Where(p => p.VendorId == vendorId)
                .ToListAsync();
        }
        public async Task<IEnumerable<Product>> GetRejectedProductsByVendorIdAsync(string vendorId)
        {
            return await GetBaseProductQuery()
                .Where(p => p.ApprovalStatus == ApprovalStatus.Rejected && p.VendorId == vendorId)
                .ToListAsync();
        }
        public async Task<IEnumerable<Product>> GetWaitingProductsByVendorIdAsync(string vendorId)
        {
            return await GetBaseProductQuery()
                .Where(p => p.ApprovalStatus == ApprovalStatus.Pending && p.VendorId == vendorId)
                .ToListAsync();
        }
    }
}
