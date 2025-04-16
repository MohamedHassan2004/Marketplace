using Marketplace.DAL.Enums;
using Marketplace.DAL.Models.Users;
using Marketplace.DAL.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Marketplace.Services.DTOs.Product
{
    public class RejectedProductDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Quantity { get; set; }
        public string CategoryName { get; set; }
        public string VendorName { get; set; }
        public string AdminRejectedName { get; set; }
        public string? RejectionReason { get; set; }
    }
}
