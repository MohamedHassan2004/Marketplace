using Marketplace.DAL.Enums;
using Marketplace.DAL.Models;

namespace Marketplace.Services.DTOs.Order
{
    public class OrderWithDetailsDto
    {
        public int Id { set; get; }
        public string CustomerId { set; get; }
        public string CustomerName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ConfirmedAt { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal ShippingCost { get; set; }
        public string ShippingAddress { get; set; }
        public OrderStatus Status { get; set; }
        public string OrderNotes { get; set; }
        public ICollection<OrderItemDto> OrderItems { set; get; } = new HashSet<OrderItemDto>();
    }

    public class OrderItemDto
    {
        public int ProductId { get; set; }
        public string ProductTitle { get; set; }
        public string ProductDescription { get; set; }
        public decimal ProductPrice { get; set; }
        public string ProductImageUrl { get; set; }
        public int OrderItemQuantity { get; set; }
        public decimal OrderItemTotalPrice { get; set; }
    }

}
