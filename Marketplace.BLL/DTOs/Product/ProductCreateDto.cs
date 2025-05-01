using Marketplace.DAL.Enums;
using Marketplace.DAL.Models.Users;
using Marketplace.DAL.Models;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Marketplace.Services.DTOs.Product
{
    public class ProductCreateDto
    {
        [StringLength(50)]
        [Required]
        public required string Title { get; set; }
        [StringLength(200)]
        public string? Description { get; set; }
        [Range(0.01, 10000.00, ErrorMessage = "Price must be between 0.01 and 10000.00")]
        public decimal Price { get; set; }
        [FromForm]
        [Required]
        public required IFormFile Image { get; set; }
        [Range(0, 1000)]
        public int Quantity { get; set; }
        public int CategoryId { get; set; }
    }
}
