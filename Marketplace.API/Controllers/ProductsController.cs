using Marketplace.DAL.Enums;
using Marketplace.DAL.Models;
using Marketplace.Services.DTOs;
using Marketplace.Services.DTOs.Product;
using Marketplace.Services.IService;
using Marketplace.Filters;
using Marketplace.Services.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;
using Marketplace.BLL.IService;

namespace Marketplace.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly INotificationService _notificationService;

        public ProductsController(IProductService productService, INotificationService notificationService)
        {
            _productService = productService;
            _notificationService = notificationService;
        }

        [HttpPost]
        [Authorize(Roles = "Vendor")]
        [RequirePermission(Permissions.AddProduct)]
        public async Task<IActionResult> AddProduct([FromForm] ProductCreateDto productDto)
        {
            var vendorId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var result = await _productService.AddProductAsync(productDto,vendorId);
            return result ? Ok("Product added successfully.") : BadRequest("Failed to add product.");
        }

        [RequirePermission(Permissions.EditProduct)]
        [Authorize(Roles = "Vendor")]
        [HttpPut("{id}")] 
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductUpdateDto productDto)
        {
            var result = await _productService.UpdateProductAsync(id, productDto);
            if (!result)
                return NotFound();

            return Ok(new { message = "Updated Successfully" });
        }

        [Authorize(Roles = "Vendor")]
        [HttpPatch("{productId}/quantity")]
        [RequirePermission(Permissions.EditProduct)]
        public async Task<IActionResult> PatchQuantityAsync(int productId, [FromBody] PatchQuantityDto model)
        {
            if (model == null)
                return BadRequest(new { Message = "Invalid request body." });

            if (model.Quantity < 0)
                return BadRequest(new { Message = "Quantity cannot be negative." });
            var result = await _productService.UpdateQuantityAsync(productId, model.Quantity);
            if (!result)
                return NotFound(new { Message = "Product not found or update failed." });

            return Ok(new { Message = "Product quantity updated successfully." });
        }

        [Authorize(Roles = "Vendor")]
        [HttpPatch("update-img/{id}")]
        [RequirePermission(Permissions.EditProduct)]
        public async Task<IActionResult> UpdateImage(int id, IFormFile newImg)
        {
            var result = await _productService.UpdateProductImgAsync(id, newImg);

            if (!result)
                return BadRequest(new { message = "Failed to update image" });

            return Ok(new { message = "Image updated successfully" });
        }

        [Authorize(Roles = "Vendor")]
        [HttpGet("{productId}/history")]
        [RequirePermission(Permissions.ViewProductHistory)]
        public async Task<ActionResult<IEnumerable<ProductHistoryDto>>> GetProductHistory(int productId)
        {
            var history = await _productService.GetProductHistoryAsync(productId);

            if (history == null || !history.Any())
            {
                return NotFound($"No history found for product with ID {productId}.");
            }

            return Ok(history);
        }


        [Authorize(Roles = "Vendor,Admin")]
        [HttpDelete("{id}")]
        [RequirePermission(Permissions.DeleteProduct)]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var result = await _productService.DeleteProductAsync(id);
            return result ? Ok("Product deleted successfully.") : NotFound("Product not found.");
        }


        [HttpGet("accepted")]
        public async Task<IActionResult> GetAcceptedProducts()
        {
            var role = User.FindFirstValue(ClaimTypes.Role);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var products = await _productService.GetAcceptedProductsAsync(role,userId);
            return Ok(products);
        }

        [HttpGet("accepted/{productOrCategoryName}")]
        public async Task<IActionResult> GetAcceptedProductsByProductOrCategoryName(string productOrCategoryName)
        {
            var role = User.FindFirstValue(ClaimTypes.Role);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var products = await _productService.GetProductsByProductNameOrCategoryNameAsync(role, userId, productOrCategoryName);
            return Ok(products);
        }

        [HttpGet("accepted/vendor/{vendorId}")]
        public async Task<IActionResult> GetAcceptedProductsByVendor(string vendorId)
        {
            var role = User.FindFirstValue(ClaimTypes.Role);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var products = await _productService.GetAcceptedProductsByVendorIdAsync(role, userId, vendorId);
            return Ok(products);
        }

        [HttpGet("accepted/price-range")]
        public async Task<IActionResult> GetProductsByPriceRange(
            [FromQuery] decimal minPrice,
            [FromQuery] decimal maxPrice)
        {
            var role = User.FindFirstValue(ClaimTypes.Role);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var products = await _productService.GetProductsByPriceRangeAsync(role, userId, minPrice, maxPrice);
            return Ok(products);
        }

        [HttpGet("accepted/category/id/{categoryId}")]
        public async Task<IActionResult> GetProductsByCategoryId(int categoryId)
        {
            var role = User.FindFirstValue(ClaimTypes.Role);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var products = await _productService.GetProductsByCategoryIdAsync(role, userId, categoryId);
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var role = User.FindFirstValue(ClaimTypes.Role);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var product = await _productService.GetProductByIdAsync(role, userId, id);
            return Ok(product);
        }

        // Admin & Vendor
        [Authorize(Roles = "Admin,Vendor")]
        [HttpGet("rejected/vendor/{vendorId}")]
        public async Task<IActionResult> GetRejectedByVendor(string vendorId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var products = await _productService.GetRejectedProductsByVendorIdAsync(userId,vendorId);
            return Ok(products);
        }

        [Authorize(Roles = "Admin,Vendor")]
        [HttpGet("waiting/vendor/{vendorId}")]
        public async Task<IActionResult> GetWaitingByVendor(string vendorId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var products = await _productService.GetWaitingProductsByVendorIdAsync(userId, vendorId);
            return Ok(products);
        }

        [Authorize(Roles = "Admin,Vendor")]
        [HttpGet("all/vendor/{vendorId}")]
        public async Task<IActionResult> GetAllProductsByVendor(string vendorId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var products = await _productService.GetAllProductsByVendorIdAsync(userId,vendorId);
            return Ok(products);
        }


        // Admin only
        [Authorize(Roles = "Admin")]
        [HttpPut("status/{productId}")]
        public async Task<IActionResult> UpdateProductStatus(int productId, [FromBody] ProductStatusUpdateDto dto)
        {
            var result = await _productService.UpdateProductStatusAsync(productId, dto);
            var product = await _productService.GetProductByIdAsync("Admin", null, productId);

            // send notification
            var notificationMessage = dto.Status == ApprovalStatus.Approved
                ? "Your product has been accepted."
                : "Your product has been rejected.";
            await _notificationService.SendNotificationAsync(product.VendorId, notificationMessage);
            return result ? Ok("Product status updated.") : NotFound("Product not found.");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("all-rejected")]
        public async Task<IActionResult> GetAllRejected()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var products = await _productService.GetRejectedProductsAsync(userId);
            return Ok(products);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("all-waiting")]
        public async Task<IActionResult> GetAllWaiting()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var products = await _productService.GetWaitingProductsAsync(userId);
            return Ok(products);
        }


        [Authorize(Roles = "Admin")]
        [HttpGet("admin/all")]
        public async Task<IActionResult> GetAllProducts()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var products = await _productService.GetAllProductsAsync(userId);
            return Ok(products);
        }

        [HttpPost("views/{id}")]
        public async Task<IActionResult> IncrementViews(int id)
        {
            var result = await _productService.IncreamentViewsNumber(id);
            return result ? Ok("Views updated.") : NotFound("Product not found.");
        }
    }
}

