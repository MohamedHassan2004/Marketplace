using Marketplace.DAL.Context;
using Marketplace.DAL.Enums;
using Marketplace.DAL.IRepository;
using Marketplace.DAL.Models;
using Marketplace.DAL.Models.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace Marketplace.DAL.Repository
{
    public class OrderRepository : Repo<Order>, IOrderRepository
    {
        private readonly MarketplaceDbContext _dbContext;

        public OrderRepository(MarketplaceDbContext context) : base(context)
        {
            _dbContext = context;
        }

        public async Task<IEnumerable<Order>> GetCompletedOrdersByCustomerAsync(string customerId)
        {
            return await _dbContext.Orders
                    .Where(o => o.CustomerId == customerId && o.Status == OrderStatus.Completed)
                    .AsNoTracking().ToListAsync();
        }

        public async Task<Order> GetInCartOrderByCustomerAsync(string customerId)
        {
            return await _dbContext.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.CustomerId == customerId && o.Status == OrderStatus.InCart);
        }

        public async Task<Order> GetOrderByIdWithDetailsAsync(int orderId)
        {
            return await _dbContext.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == orderId);
        }
    }
}
