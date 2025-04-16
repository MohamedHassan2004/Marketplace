using Marketplace.DAL.Models.Users;
using Marketplace.DAL.Models;

namespace Marketplace.Services.DTOs.Product
{
    public class WaitingProductDto
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
    }
}
