using Marketplace.BLL.IService;
using Marketplace.DAL.WebSockets;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        private readonly WebSocketConnectionManager _connectionManager;

        public NotificationsController(WebSocketConnectionManager connectionManager, INotificationService notificationService)
        {
            _connectionManager = connectionManager;
            _notificationService = notificationService;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("broadcast")]
        public async Task<IActionResult> BroadcastNotification([FromBody] string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                return BadRequest("Message cannot be empty.");
            }

            await _connectionManager.BroadcastMessageAsync(message);
            return Ok("Notification sent to all vendors.");
        }

        [Authorize(Roles = "Vendor")]
        [HttpGet("Vendor/{userId}")]
        public async Task<IActionResult> GetNotifications(string userId)
        {
            var notifications = await _notificationService.GetNotificationsByUserIdAsync(userId);
            return Ok(notifications);
        }

        [Authorize(Roles = "Vendor")]
        [HttpDelete("{notificationId}")]
        public async Task<IActionResult> DeleteNotification(int notificationId)
        {
            var result = await _notificationService.DeleteNotificationAsync(notificationId);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
