using Marketplace.DAL.Models;
using Marketplace.Services.DTOs;
using Marketplace.Services.DTOs.Product;
using Marketplace.Services.IService;
using Marketplace.Services.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Marketplace.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [Authorize(Roles = "Vendor")]
        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] ProductCreateDto productDto)
        {
            var vendorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _productService.AddProductAsync(productDto,vendorId);
            return result ? Ok("Product added successfully.") : BadRequest("Failed to add product.");
        }

        [Authorize(Roles = "Vendor,Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var result = await _productService.DeleteProductAsync(id);
            return result ? Ok("Product deleted successfully.") : NotFound("Product not found.");
        }

        [Authorize(Roles = "Vendor")]
        [HttpPut("{id}")] 
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductCreateDto productDto)
        {
            var result = await _productService.UpdateProductAsync(id, productDto);
            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpGet("accepted")]
        public async Task<IActionResult> GetAcceptedProducts()
        {
            var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var products = await _productService.GetAcceptedProductsAsync(customerId);
            return Ok(products);
        }

        [HttpGet("accepted/vendor/{vendorId}")]
        public async Task<IActionResult> GetAcceptedProductsByVendor(string vendorId)
        {
            var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var products = await _productService.GetAcceptedProductsByVendorIdAsync(customerId, vendorId);
            return Ok(products);
        }

        [HttpGet("accepted/price-range")]
        public async Task<IActionResult> GetProductsByPriceRange(
            [FromQuery] decimal minPrice,
            [FromQuery] decimal maxPrice)
        {
            var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var products = await _productService.GetProductsByPriceRangeAsync(customerId, minPrice, maxPrice);
            return Ok(products);
        }

        [HttpGet("accepted/category/{categoryName}")]
        public async Task<IActionResult> GetProductsByCategoryName(string categoryName)
        {
            var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var products = await _productService.GetProductsByCategoryNameAsync(customerId, categoryName);
            return Ok(products);
        }

        [HttpGet("accepted/category/{categoryId}")]
        public async Task<IActionResult> GetProductsByCategoryId(int categoryId)
        {
            var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var products = await _productService.GetProductsByCategoryIdAsync(customerId, categoryId);
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAcceptedProductById(int id)
        {
            var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var product = await _productService.GetAcceptedProductByIdAsync(customerId, id);
            return Ok(product);
        }

        // Admin & Vendor
        [Authorize(Roles = "Admin,Vendor")]
        [HttpGet("rejected/vendor/{vendorId}")]
        public async Task<IActionResult> GetRejectedByVendor(string vendorId)
        {
            var products = await _productService.GetRejectedProductsByVendorIdAsync(vendorId);
            return Ok(products);
        }

        [Authorize(Roles = "Admin,Vendor")]
        [HttpGet("waiting/vendor/{vendorId}")]
        public async Task<IActionResult> GetWaitingByVendor(string vendorId)
        {
            var products = await _productService.GetWaitingProductsByVendorIdAsync(vendorId);
            return Ok(products);
        }

        // Admin only
        [Authorize(Roles = "Admin")]
        [HttpPut("status")]
        public async Task<IActionResult> UpdateProductStatus([FromBody] ProductStatusUpdateDto dto)
        {
            var result = await _productService.UpdateProductStatusAsync(dto);
            return result ? Ok("Product status updated.") : NotFound("Product not found.");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("all-rejected")]
        public async Task<IActionResult> GetAllRejected()
        {
            var products = await _productService.GetRejectedProductsAsync();
            return Ok(products);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("all-waiting")]
        public async Task<IActionResult> GetAllWaiting()
        {
            var products = await _productService.GetWaitingProductsAsync();
            return Ok(products);
        }

        [HttpPost("views/{id}")]
        public async Task<IActionResult> IncrementViews(int id)
        {
            var result = await _productService.IncreamentViewsNumber(id);
            return result ? Ok("Views updated.") : NotFound("Product not found.");
        }

        [HttpGet("admin/all")]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }
    }
}

