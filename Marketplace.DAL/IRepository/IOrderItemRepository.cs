using Marketplace.DAL.Models;

namespace Marketplace.DAL.IRepository
{
    public interface IOrderItemRepository : IRepo<OrderItem>
    {
        Task<IEnumerable<OrderItem>> GetProductHistoryAsync(int productId);
    }
}
