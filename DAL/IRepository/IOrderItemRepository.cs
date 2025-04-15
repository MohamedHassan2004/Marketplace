using Marketplace.DAL.Models;

namespace Marketplace.DAL.IRepository
{
    public interface IOrderItemRepository : IRepo<OrderItem>
    {
        Task<OrderItem> GetOrderItemByIdWithDetailsAsync(int orderItemId);
        Task<IEnumerable<OrderItem>> GetOrderItemsWithDetailsAsync();
    }
}
