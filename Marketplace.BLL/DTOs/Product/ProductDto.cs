using Marketplace.DAL.Models.Users;
using Marketplace.DAL.Models;
using System.ComponentModel.DataAnnotations.Schema;
using Marketplace.DAL.Enums;

namespace Marketplace.Services.DTOs.Product
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Quantity { get; set; }
        public int ViewsNumber { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string VendorId { get; set; }
        public string VendorName { get; set; }
        public ApprovalStatus ApprovalStatus { get; set; }
        public string AdminCheckedName { get; set; }
        public string? RejectionReason { get; set; }
        public bool CanBeDeleted { get; set; } = false;
        public bool CanBeUpdated { get; set; } = false;
        public bool CanBuy { get; set; } = false;
    }
}
