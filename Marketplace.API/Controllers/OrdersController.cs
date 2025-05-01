using Microsoft.AspNetCore.Mvc;
using Marketplace.Services.DTOs.Order;
using Marketplace.Services.IService;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Marketplace.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [Authorize(Roles = "Customer")]
        [HttpPost("create")]
        public async Task<IActionResult> CreateOrder()
        {
            var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _orderService.CreateOrderAsync(customerId);
            if (!result)
                return StatusCode(500, "Failed to create order.");

            return Ok("Order created successfully.");
        }

        [Authorize(Roles = "Customer")]
        [HttpPost("{orderId}/confirm")]
        public async Task<IActionResult> ConfirmOrder(int orderId, [FromBody] ConfirmOrderDto confirmDto)
        {
            try
            {
                var result = await _orderService.ConfirmOrderAsync(orderId, confirmDto);
                if (!result)
                    return NotFound("Order not found or already confirmed");

                return Ok("Order confirmed successfully");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while confirming the order.");
            }
        }

        [Authorize(Roles = "Customer")]
        [HttpDelete("{orderId}")]
        public async Task<IActionResult> DeleteOrder(int orderId)
        {
            var result = await _orderService.DeleteOrderAsync(orderId);
            if (!result)
                return NotFound($"Order with ID {orderId} not found.");

            return Ok("Order deleted successfully.");
        }


        [Authorize(Roles = "Customer")]
        [HttpGet("has-cart")]
        public async Task<IActionResult> HasCart()
        {
            var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var order = await _orderService.GetInCartOrderByCustomerIdAsync(customerId);
            if(order == null)
                return NotFound("No cart found for the customer.");
            return Ok(order.Id);
        }


        [Authorize(Roles = "Customer")]
        [HttpGet("incart")]
        public async Task<IActionResult> GetInCartOrder()
        {
            var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var order = await _orderService.GetInCartOrderByCustomerIdAsync(customerId);
            return Ok(order);
        }

        [HttpGet("completed")]
        public async Task<IActionResult> GetCompletedOrders([FromQuery] string customerId)
        {
            var orders = await _orderService.GetCompletedOrdersByCustomerIdAsync(customerId);
            return Ok(orders);
        }

        [HttpGet("details/{orderId}")]
        public async Task<IActionResult> GetOrderDetails(int orderId)
        {
            var orderDetails = await _orderService.GetOrderWithDetailsByIdAsync(orderId);
            if (orderDetails == null)
                return NotFound($"Order with ID {orderId} not found.");
            return Ok(orderDetails);
        }
    }
}
