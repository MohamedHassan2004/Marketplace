using Marketplace.Services.DTOs.Order;
using Marketplace.Services.DTOs.Product;

namespace Marketplace.Services.IService
{
    public interface IOrderItemService
    {
        Task<bool> AddOrderItemAsync(CreateOrderItemDto orderItemDto);
        Task<bool> UpdateOrderItemQuantityAsync(int orderItemId, int newQuantity);
        Task<bool> DeleteOrderItemAsync(int orderItemId);
    }
}
