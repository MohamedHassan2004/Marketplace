using Marketplace.DAL.Models;

namespace Marketplace.DAL.IRepository
{
    public interface IOrderItemRepository : IRepo<OrderItem>
    {
        Task<OrderItem> GetOrderItemByIdWithDetails(int orderItemId);
        Task<IEnumerable<OrderItem>> GetOrderItemsWithDetails();
    }
}
