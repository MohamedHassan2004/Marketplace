using Marketplace.DAL.Context;
using Marketplace.DAL.IRepository;
using Marketplace.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Socialify.DAL.Repository;

namespace Marketplace.DAL.Repository
{
    public class OrderItemRepository : Repo<OrderItem>, IOrderItemRepository
    {
        private readonly MarketplaceDbContext _dbContext;

        public OrderItemRepository(MarketplaceDbContext context) : base(context)
        {
            _dbContext = context;
        }

        public async Task<IEnumerable<OrderItem>> GetOrderItemsWithDetails()
        {
            return await _dbContext.OrderItems
                .Include(oi => oi.Product)
                .Include(oi => oi.Order)
                .ToListAsync();
        }

        public async Task<OrderItem> GetOrderItemByIdWithDetails(int orderItemId)
        {
            return await _dbContext.OrderItems
                .Include(oi => oi.Product)
                .Include(oi => oi.Order)
                .FirstOrDefaultAsync(oi => oi.Id == orderItemId);
        }
    }

}
