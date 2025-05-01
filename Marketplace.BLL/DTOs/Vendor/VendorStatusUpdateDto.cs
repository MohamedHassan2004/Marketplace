using Marketplace.DAL.Enums;
using System.ComponentModel.DataAnnotations;

namespace Marketplace.Services.DTOs.Vendor
{
    public class VendorStatusUpdateDto
    {
        [StringLength(500)]
        public string? RejectionReason { get; set; }
        public ApprovalStatus Status { get; set; }
    }
}
