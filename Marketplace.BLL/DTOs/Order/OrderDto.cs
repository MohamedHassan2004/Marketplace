using Marketplace.DAL.Enums;

namespace Marketplace.Services.DTOs.Order
{
    public class OrderDto
    {
        public int Id { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ConfirmedAt { get; set; }
        public string CustomerId { get; set; }
        public decimal TotalAmount { get; set; }
        public string OrderNotes { get; set; }
    }
}
