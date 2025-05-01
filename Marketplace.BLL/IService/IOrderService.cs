using Marketplace.Services.DTOs.Order;

namespace Marketplace.Services.IService
{
    public interface IOrderService
    {
        Task<bool> CreateOrderAsync(string customerId);
        Task<bool> DeleteOrderAsync(int orderId);
        Task<bool> ConfirmOrderAsync(int orderId, ConfirmOrderDto orderDto);
        Task<OrderWithDetailsDto> GetOrderWithDetailsByIdAsync(int orderId);
        Task<OrderWithDetailsDto> GetInCartOrderByCustomerIdAsync(string customerId);
        Task<IEnumerable<OrderDto>> GetCompletedOrdersByCustomerIdAsync(string customerId);

    }
}
