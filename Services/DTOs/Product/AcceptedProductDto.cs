using Marketplace.DAL.Models.Users;
using Marketplace.DAL.Models;
using System.ComponentModel.DataAnnotations.Schema;
using Marketplace.DAL.Enums;

namespace Marketplace.Services.DTOs.Product
{
    public class AcceptedProductDto
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
        public string AdminApprovedName { get; set; }
        public bool IsSaved { get; set; }
        public float AverageRating { get; set; }
    }
}
