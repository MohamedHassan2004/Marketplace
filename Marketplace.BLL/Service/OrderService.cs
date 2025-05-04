using Marketplace.DAL.Enums;
using Marketplace.DAL.IRepository;
using Marketplace.DAL.Models;
using Marketplace.Services.DTOs.Order;
using Marketplace.Services.IService;
using AutoMapper;

namespace Marketplace.Services.Service
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public OrderService(IOrderRepository orderRepository, IProductService productService, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _productService = productService;
            _mapper = mapper;
        }

        public async Task<int> CreateOrderAsync(string customerId)
        {
            var order = new Order() { CustomerId = customerId};
            await _orderRepository.AddAsync(order);
            return order.Id;
        }

        public async Task<bool> ConfirmOrderAsync(int orderId, ConfirmOrderDto orderDto)
        {
            var orderEntity = await _orderRepository.GetByIdAsync(orderId);
            if (orderEntity == null)
                return false;

            if (orderEntity.Status != OrderStatus.InCart)
                throw new InvalidOperationException("Order has already been confirmed.");

            if (orderEntity.OrderItems == null || !orderEntity.OrderItems.Any())
                throw new InvalidOperationException("Cannot confirm an empty order.");

            foreach (var item in orderEntity.OrderItems)
            {
                if (item.Product == null)
                    throw new InvalidOperationException("Product not loaded in order item.");

                if (item.Product.Quantity < item.Quantity)
                    throw new InvalidOperationException($"Not enough quantity for product {item.ProductId}.");
            }

            foreach (var item in orderEntity.OrderItems)
            {
                await _productService.UpdateQuantityAsync(item.ProductId, -item.Quantity);
            }

            orderEntity.ConfirmedAt = DateTime.UtcNow;
            orderEntity.Status = OrderStatus.Completed;
            orderEntity.ShippingCost = Consts.ShipingCost;
            orderEntity.ShippingAddress = orderDto.ShippingAddress;
            orderEntity.OrderNotes = orderDto.OrderNotes;

            return await _orderRepository.UpdateAsync(orderEntity);
        }


        public async Task<IEnumerable<OrderDto>> GetCompletedOrdersByCustomerIdAsync(string customerId)
        {
            var orderEntities = await _orderRepository.GetCompletedOrdersByCustomerAsync(customerId);
            var dtos = _mapper.Map<IEnumerable<OrderDto>>(orderEntities);
            return dtos;
        }
        public async Task<OrderWithDetailsDto> GetInCartOrderByCustomerIdAsync(string customerId)
        {
            var orderEntity = await _orderRepository.GetInCartOrderByCustomerAsync(customerId);
            OrderWithDetailsDto orderWithDetailsDto;
            return ConvertToOrderWithDetailsDto(orderEntity, out orderWithDetailsDto);
        }

        public async Task<OrderWithDetailsDto> GetOrderWithDetailsByIdAsync(int orderId)
        {
            var orderWithDetailsEntity = await _orderRepository.GetOrderByIdWithDetailsAsync(orderId);
            OrderWithDetailsDto orderWithDetailsDto;
            return ConvertToOrderWithDetailsDto(orderWithDetailsEntity, out orderWithDetailsDto);
        }

        private static OrderWithDetailsDto ConvertToOrderWithDetailsDto(Order orderWithDetailsEntity, out OrderWithDetailsDto orderWithDetailsDto)
        {
            if (orderWithDetailsEntity == null)
            {
                orderWithDetailsDto = null;
                return orderWithDetailsDto;
            }

            var totalAmount = orderWithDetailsEntity.OrderItems.Sum(item => item.Quantity * item.Product.Price);

            orderWithDetailsDto = new OrderWithDetailsDto()
            {
                Id = orderWithDetailsEntity.Id,
                CustomerId = orderWithDetailsEntity.CustomerId,
                CreatedAt = orderWithDetailsEntity.CreatedAt,
                Status = orderWithDetailsEntity.Status,
                ConfirmedAt = orderWithDetailsEntity.ConfirmedAt,
                ShippingCost = orderWithDetailsEntity.ShippingCost,
                ShippingAddress = orderWithDetailsEntity.ShippingAddress,
                OrderNotes = orderWithDetailsEntity.OrderNotes,
                TotalAmount = totalAmount + Consts.ShipingCost,
                OrderItems = orderWithDetailsEntity.OrderItems.Select(oi => new OrderItemDto
                {
                    ProductId = oi.ProductId,
                    ProductTitle = oi.Product.Title,
                    ProductDescription = oi.Product.Description ?? "",
                    ProductPrice = oi.Product.Price,
                    ProductImageUrl = oi.Product.ImageUrl,
                    OrderItemQuantity = oi.Quantity,
                    OrderItemTotalPrice = oi.Product.Price * oi.Quantity
                }).ToList(),
            };

            return orderWithDetailsDto;
        }
    }
}
