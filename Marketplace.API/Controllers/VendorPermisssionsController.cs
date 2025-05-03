using Marketplace.BLL.IService;
using Marketplace.DAL.Enums;
using Marketplace.Services.DTOs;
using Marketplace.Services.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Marketplace.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class VendorPermissionController : ControllerBase
    {
        private readonly IVendorPermissionService _vendorPermissionService;
        private readonly INotificationService _notificationService;

        public VendorPermissionController(IVendorPermissionService vendorPermissionService, INotificationService notificationService)
        {
            _vendorPermissionService = vendorPermissionService;
            _notificationService = notificationService;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AssignPermissionToVendor([FromBody] CreateVendorPermissionDto dto)
        {
            var adminId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            if (string.IsNullOrWhiteSpace(dto.VendorId) || dto.PermissionId == 0)
                return BadRequest(new { Message = "VendorId and PermissionId are required." });

            var result = await _vendorPermissionService.AssignPermissionToVendorAsync(dto,adminId);

            if (!result)
                return StatusCode(500, new { Message = "Failed to assign permission to vendor." });

            // send notificatioin
            var permissionName = Enum.GetName(typeof(Permissions), dto.PermissionId);
            await _notificationService.SendNotificationAsync(dto.VendorId, $"Now You have this permission: {permissionName}.");

            return Ok(new { Message = "Permission assigned to vendor successfully." });
        }

        [Authorize(Roles = "Admin, Vendor")]
        [HttpGet("{vendorId}")]
        public async Task<IActionResult> GetVendorPermissions(string vendorId)
        {
            var permissions = await _vendorPermissionService.GetVendorWithPermissionsDetailsAsync(vendorId);
            if (!permissions.Any())
                return NotFound(new { Message = "No permissions found for the given vendor." });

            return Ok(permissions);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemovePermission(int id)
        {
            var result = await _vendorPermissionService.RemovePermissionFromVendorAsync(id); 
            var vendorPermission = await _vendorPermissionService.GetVendorPermissionByIdAsync(id);

            if (!result)
                return NotFound(new { Message = "Permission not found or already removed." });

            // send notificatioin
            var permissionName = Enum.GetName(typeof(Permissions), id);
            await _notificationService.SendNotificationAsync(vendorPermission.VendorId, $"Now You don't have this permission: {permissionName}.");

            return Ok(new { Message = "Permission removed from vendor successfully." });
        }
    }
}
