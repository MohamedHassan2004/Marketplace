using Marketplace.DAL.Enums;

namespace Marketplace.Services.DTOs.Vendor
{
    public class VendorDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public ApprovalStatus ApprovalStatus { get; set; }
        public string? AdminIdChecked { get; set; }
        public string? RejectionReason { get; set; }
        public float Rating { get; set; }
    }
}
