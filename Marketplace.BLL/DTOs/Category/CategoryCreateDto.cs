using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Marketplace.Services.DTOs.Category
{
    public class CategoryCreateDto
    {
        [Required]
        public required string Name { get; set; }
        [Required]
        [FromForm]
        public IFormFile Image { get; set; }
    }
}
