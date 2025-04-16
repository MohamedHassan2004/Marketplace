using System.ComponentModel.DataAnnotations.Schema;
using Marketplace.DAL.Enums;
using Marketplace.DAL.Models.Users;

namespace Marketplace.DAL.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int Quantity { get; set; }
        public int ViewsNumber { get; set; } = 0;
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
        public string VendorId { get; set; }
        [ForeignKey("VendorId")]
        public Vendor Vendor { get; set; }
        public ApprovalStatus ApprovalStatus { get; set; } = ApprovalStatus.Pending;
        public string? AdminCheckedId { get; set; }
        [ForeignKey("AdminCheckedId")]
        public Admin? AdminChecked { get; set; }
        public string? RejectionReason { get; set; }
        public ICollection<SavedProduct> SavedProducts { get; set; } = new List<SavedProduct>();
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<ProductReview> Reviews { get; set; } = new List<ProductReview>();
    }
}
