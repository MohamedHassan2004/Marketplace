using Marketplace.DAL.Enums;
using System.ComponentModel.DataAnnotations;

namespace Marketplace.Services.DTOs.Product
{
    public class ProductStatusUpdateDto
    {
        [StringLength(500)]
        public string? RejectionReason { get; set; }
        public ApprovalStatus Status { get; set; }
        public string AdminCheckedId { get; set; }
    }
}
