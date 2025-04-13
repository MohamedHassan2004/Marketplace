using System.ComponentModel.DataAnnotations.Schema;
using Marketplace.DAL.Enums;
using Marketplace.DAL.Models.Users;

namespace Marketplace.DAL.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string CustomerId { get; set; }
        public string VendorId { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal ShippingCost { get; set; }
        public string ShippingAddress { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public DateTime? DeliveredDate { get; set; }
        public string OrderNotes { get; set; } = string.Empty;
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }
        [ForeignKey("VendorId")]
        public Vendor Vendor { get; set; }

    }
}
