using Marketplace.DAL.Context;
using Marketplace.DAL.Enums;
using Marketplace.DAL.IRepository;
using Marketplace.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace Marketplace.DAL.Repository
{
    public class OrderItemRepository : Repo<OrderItem>, IOrderItemRepository
    {
        private readonly MarketplaceDbContext _dbContext;

        public OrderItemRepository(MarketplaceDbContext context) : base(context)
        {
            _dbContext = context;
        }

        public new async Task<IEnumerable<OrderItem>> GetAllAsync()
        {
            return await _dbContext.OrderItems
                .Include(oi => oi.Product)
                .AsNoTracking()
                .ToListAsync();
        }

        public new async Task<OrderItem> GetByIdAsync(int id)
        {
            return await _dbContext.OrderItems
                .Include(oi => oi.Product)
                .FirstOrDefaultAsync(oi => oi.Id == id);
        }

        public async Task<IEnumerable<OrderItem>> GetProductHistoryAsync(int productId)
        {
            return await _dbContext.OrderItems
                .Include(oi => oi.Order)
                .ThenInclude(o => o.Customer)
                .Where(oi => oi.Order.Status == OrderStatus.Completed && oi.ProductId == productId)
                .AsNoTracking()
                .ToListAsync();
        }
    }

}
