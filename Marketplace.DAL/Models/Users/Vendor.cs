using Marketplace.DAL.Enums;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Marketplace.DAL.Models.Users
{
    public class Vendor : ApplicationUser
    {
        public ApprovalStatus ApprovalStatus { get; set; } = ApprovalStatus.Pending;
        public string? AdminIdChecked { get; set; }
        [ForeignKey("AdminIdApproved")]
        public Admin? AdminChecked { get; set; }
        public string? RejectionReason { get; set; }
        public float Rating { get; set; } = 0;
        public ICollection<Product> Products { get; set; } = new List<Product>();
        public ICollection<VendorPermission> Permissions { get; set; } = new List<VendorPermission>();
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
