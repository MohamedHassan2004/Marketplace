using Microsoft.AspNetCore.Mvc;
using Marketplace.Services.DTOs.Order;
using Marketplace.Services.IService;
using Microsoft.AspNetCore.Authorization;
using Marketplace.Services.DTOs;

namespace Marketplace.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Customer")]
    public class OrderItemsController : ControllerBase
    {
        private readonly IOrderItemService _orderItemService;

        public OrderItemsController(IOrderItemService orderItemService)
        {
            _orderItemService = orderItemService;
        }

        [HttpPost]
        public async Task<IActionResult> AddOrderItem([FromBody] CreateOrderItemDto orderItemDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _orderItemService.AddOrderItemAsync(orderItemDto);
            if (!result)
                return StatusCode(500, "Failed to add order item.");

            return Ok("Order item added successfully.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderItem(int id)
        {
            var result = await _orderItemService.DeleteOrderItemAsync(id);
            if (!result)
                return NotFound($"Order item with ID {id} not found.");

            return Ok("Order item deleted successfully.");
        }

        [HttpPut("{id}/quantity")]
        public async Task<IActionResult> UpdateOrderItemQuantity(int id, [FromBody] PatchQuantityDto model)
        {
            if (model == null)
                return BadRequest(new { Message = "Invalid request body." });

            if (model.Quantity < 0)
                return BadRequest(new { Message = "Quantity cannot be negative." });
            var result = await _orderItemService.UpdateOrderItemQuantityAsync(id, model.Quantity);
            if (!result)
                return BadRequest("Failed to update quantity. Make sure the order item exists and the quantity is valid.");

            return Ok("Order item quantity updated successfully.");
        }
    }
}
