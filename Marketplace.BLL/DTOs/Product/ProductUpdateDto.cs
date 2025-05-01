using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Marketplace.Services.DTOs.Product
{
    public class ProductUpdateDto
    {
        [StringLength(50)]
        public string Title { get; set; }
        [StringLength(200)]
        public string? Description { get; set; }
        [Range(0.01, 10000.00, ErrorMessage = "Price must be between 0.01 and 10000.00")]
        public decimal Price { get; set; }
        [Range(0, 1000)]
        public int Quantity { get; set; }
        public int CategoryId { get; set; }
    }
}
