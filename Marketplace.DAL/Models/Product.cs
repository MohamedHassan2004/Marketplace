using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Marketplace.DAL.Enums;
using Marketplace.DAL.Models.Users;

namespace Marketplace.DAL.Models
{
        public class Product
        {
            public int Id { get; set; }
            [StringLength(50, MinimumLength =3)]
            public required string Title { get; set; }
            public string? Description { get; set; }
            [Range(0.01, 10000.00, ErrorMessage = "Price must be between 0.01 and 10000.00")]
            public decimal Price { get; set; }
            public required string ImageUrl { get; set; }
            public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
            public int Quantity { get; set; } = 0;
            public int ViewsNumber { get; set; } = 0;
            public ApprovalStatus ApprovalStatus { get; set; } = ApprovalStatus.Pending;
            public int CategoryId { get; set; }
            public required string VendorId { get; set; }
            public string? AdminCheckedId { get; set; }
            public Category Category { get; set; }
            public Vendor Vendor { get; set; }
            public Admin? AdminChecked { get; set; }
            public string? RejectionReason { get; set; }
            public ICollection<SavedProduct> SavedProducts { get; set; } = new List<SavedProduct>();
            public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        }
}
