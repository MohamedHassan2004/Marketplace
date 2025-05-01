using Marketplace.DAL.IRepository;
using Marketplace.DAL.Models;
using Marketplace.Services.DTOs.Order;
using Marketplace.Services.IService;

namespace Marketplace.Services.Service
{
    public class OrderItemService : IOrderItemService
    {
        private readonly IOrderItemRepository _orderItemRepository;

        public OrderItemService(IOrderItemRepository orderItemRepository)
        {
            _orderItemRepository = orderItemRepository;
        }

        public async Task<bool> AddOrderItemAsync(CreateOrderItemDto orderItemDto)
        {
            var added = await _orderItemRepository.AddAsync(new OrderItem
            {
                OrderId = orderItemDto.OrderId,
                ProductId = orderItemDto.ProductId,
                Quantity = orderItemDto.Quantity
            });
            return added;
        }   

        public async Task<bool> DeleteOrderItemAsync(int orderItemId)
        {
            return await _orderItemRepository.DeleteByIdAsync(orderItemId);
        }

        public async Task<bool> UpdateOrderItemQuantityAsync(int orderItemId, int newQuantity)
        {
            var orderItem = await _orderItemRepository.GetByIdAsync(orderItemId);
            if (orderItem == null)
            {
                return false;
            }
            if (newQuantity < 0 || newQuantity > orderItem.Quantity)
            {
                return false;
            }   
            orderItem.Quantity = newQuantity;
            return await _orderItemRepository.UpdateAsync(orderItem);
        }
    }
}
