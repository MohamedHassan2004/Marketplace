using Marketplace.DAL.Models;

namespace Marketplace.DAL.IRepository
{
    public interface IOrderRepository : IRepo<Order>
    {
        Task<Order> GetOrderByIdWithDetails(int orderId);
    }
}
