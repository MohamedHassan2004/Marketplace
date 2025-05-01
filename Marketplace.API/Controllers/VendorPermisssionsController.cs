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

        public VendorPermissionController(IVendorPermissionService vendorPermissionService)
        {
            _vendorPermissionService = vendorPermissionService;
        }

        [HttpPost]
        public async Task<IActionResult> AssignPermissionToVendor([FromBody] CreateVendorPermissionDto dto)
        {
            var adminId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            if (string.IsNullOrWhiteSpace(dto.VendorId) || dto.PermissionId == 0)
                return BadRequest(new { Message = "VendorId and PermissionId are required." });

            var result = await _vendorPermissionService.AssignPermissionToVendorAsync(dto,adminId);

            if (!result)
                return StatusCode(500, new { Message = "Failed to assign permission to vendor." });

            return Ok(new { Message = "Permission assigned to vendor successfully." });
        }

        [HttpGet("{vendorId}")]
        public async Task<IActionResult> GetVendorPermissions(string vendorId)
        {
            var permissions = await _vendorPermissionService.GetVendorWithPermissionsDetailsAsync(vendorId);
            if (!permissions.Any())
                return NotFound(new { Message = "No permissions found for the given vendor." });

            return Ok(permissions);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemovePermission(int id)
        {
            var result = await _vendorPermissionService.RemovePermissionFromVendorAsync(id);

            if (!result)
                return NotFound(new { Message = "Permission not found or already removed." });

            return Ok(new { Message = "Permission removed from vendor successfully." });
        }
    }
}
