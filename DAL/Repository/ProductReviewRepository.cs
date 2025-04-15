using Marketplace.DAL.Context;
using Marketplace.DAL.IRepository;
using Marketplace.DAL.Models;
using Marketplace.DAL.Models.Users;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.DAL.Repository
{
    public class ProductReviewRepository : Repo<ProductReview>, IProductReviewRepository
    {
        private readonly MarketplaceDbContext _dbContext;
        public ProductReviewRepository(MarketplaceDbContext context) : base(context)
        {
            _dbContext = context;
        }

        public async Task<IEnumerable<ProductReview>> GetReviewsDetailsAsync()
        {
            return await _dbContext.ProductReviews
                .Include(pr => pr.Customer)
                .Include(p=> p.Product)
                .ToListAsync();
        }

        public async Task<IEnumerable<ProductReview>> GetReviewsByProductIdAsync(int productId)
        {
            return await _dbContext.ProductReviews
                .Where(pr => pr.ProductId == productId)
                .Include(pr => pr.Customer)
                .ToListAsync();
        }

        public async Task<IEnumerable<ProductReview>> GetReviewsByCustomerIdAsync(string customerId)
        {
            return await _dbContext.ProductReviews
                .Where(pr => pr.CustomerId == customerId)
                .Include(pr => pr.Customer)
                .ToListAsync();
        }


    }
}
