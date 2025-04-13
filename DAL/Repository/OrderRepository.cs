using Marketplace.DAL.Context;
using Marketplace.DAL.IRepository;
using Marketplace.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Socialify.DAL.Repository;

namespace Marketplace.DAL.Repository
{
    public class OrderRepository : Repo<Order>, IOrderRepository
    {
        private readonly MarketplaceDbContext _dbContext;

        public OrderRepository(MarketplaceDbContext context) : base(context)
        {
            _dbContext = context;
        }

        public async Task<Order> GetOrderByIdWithDetails(int orderId)
        {
            return await _dbContext.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Include(o => o.Customer)
                .FirstOrDefaultAsync(o => o.Id == orderId);
        }

    }
}
