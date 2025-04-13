using Marketplace.DAL.Context;
using Marketplace.DAL.Enums;
using Marketplace.DAL.IRepository;
using Marketplace.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Socialify.DAL.Repository;

namespace Marketplace.DAL.Repository
{
    public class ProductRepository : Repo<Product>, IProductRepository
    {
        private readonly MarketplaceDbContext _dbContext;

        public ProductRepository(MarketplaceDbContext context) : base(context)
        {
            _dbContext = context;
        }

        public async Task<Product> GetProductWithDetailsById(int Id)
        {
            return await _dbContext.Products
                .Include(p => p.Category)
                .Include(p=> p.Reviews)
                .Include(p => p.Vendor)
                .FirstOrDefaultAsync(p => p.Id == Id);
        }

    }
}
