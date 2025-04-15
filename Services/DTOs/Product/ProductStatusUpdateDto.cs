using Marketplace.DAL.Enums;

namespace Marketplace.Services.DTOs.Product
{
    public class ProductStatusUpdateDto
    {
        public int ProductId { get; set; }
        public string? RejectionReason { get; set; }
        public ApprovalStatus ApprovalStatus { get; set; }
        public string AdminCheckedId { get; set; }

    }
}
