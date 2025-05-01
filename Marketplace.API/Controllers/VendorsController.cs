using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Marketplace.Services.IService;
using Marketplace.DAL.Enums;
using Marketplace.Services.DTOs.Vendor;
using System.Security.Claims;

namespace Marketplace.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class VendorsController : ControllerBase
    {
        private readonly IVendorService _vendorService;

        public VendorsController(IVendorService vendorService)
        {
            _vendorService = vendorService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllVendors()
        {
            var vendors = await _vendorService.GetAllVendorsAsync();
            return Ok(vendors);
        }

        [HttpGet("waiting")]
        public async Task<IActionResult> GetWaitingVendors()
        {
            var vendors = await _vendorService.GetWaitingVendorsAsync();
            return Ok(vendors);
        }

        [HttpGet("{vendorId}")]
        public async Task<IActionResult> GetVendorById(string vendorId)
        {
            var vendor = await _vendorService.GetVendorByIdAsync(vendorId);
            if (vendor == null)
                return NotFound(new { Message = "Vendor not found." });

            return Ok(vendor);
        }

        [HttpPatch("{vendorId}/status")]
        public async Task<IActionResult> UpdateVendorStatus(string vendorId ,[FromBody] VendorStatusUpdateDto model)
        {
            var adminId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            if (!Enum.IsDefined(typeof(ApprovalStatus), model.Status))
                return BadRequest(new { Message = "Invalid status value." });

            var result = await _vendorService.UpdateVendorStatusAsync(vendorId, model, adminId);
            if (!result)
                return NotFound(new { Message = "Vendor not found or status update failed." });

            return Ok(new { Message = "Vendor status updated successfully." });
        }
    }

}
