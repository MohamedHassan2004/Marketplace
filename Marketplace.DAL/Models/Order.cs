using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Marketplace.DAL.Enums;
using Marketplace.DAL.Models.Users;

namespace Marketplace.DAL.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ConfirmedAt { get; set; }
        public required string CustomerId { get; set; }
        public decimal ShippingCost { get; set; } = Consts.ShipingCost;
        [StringLength(200)]
        public string? ShippingAddress { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.InCart;
        [StringLength(200)]
        public string OrderNotes { get; set; } = string.Empty;
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public Customer Customer { get; set; }

    }
}
