using Marketplace.Services.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Marketplace.Controllers
{
    [Authorize(Roles = "Customer")]
    [ApiController]
    [Route("api/[controller]")]
    public class SavedProductsController : ControllerBase
    {
        private readonly ISavedProductService _savedProductService;

        public SavedProductsController(ISavedProductService savedProductService)
        {
            _savedProductService = savedProductService;
        }

        [HttpGet]
        public async Task<IActionResult> GetSavedProducts()
        {
            var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(customerId)) return Unauthorized();

            var savedProducts = await _savedProductService.GetSavedProductsAsync(customerId);
            return Ok(savedProducts);
        }

        [HttpPost("{productId}")]
        public async Task<IActionResult> SaveProduct(int productId)
        {
            var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(customerId)) return Unauthorized();

            var success = await _savedProductService.SaveProduct(productId, customerId);
            if (!success) return NotFound("Product not found or already saved.");
            return Ok("Product saved successfully.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> UnsaveProduct(int id)
        {
            var success = await _savedProductService.UnsaveProduct(id);
            if (!success) return NotFound("Product not found or not saved.");
            return Ok("Product unsaved successfully.");
        }
    }
}

