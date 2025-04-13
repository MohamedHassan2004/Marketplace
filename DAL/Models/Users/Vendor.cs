using Marketplace.DAL.Enums;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Marketplace.DAL.Models.Users
{
    public class Vendor : ApplicationUser
    {
        public ApprovalStatus ApprovalStatus { get; set; } = ApprovalStatus.Pending;
        public string? AdminIdApproved { get; set; }
        [ForeignKey("AdminIdApproved")]
        public Admin? AdminApproved { get; set; }
        public string? RejectionReason { get; set; }
        public ICollection<Product> Products { get; set; } = new List<Product>();
        public ICollection<VendorPermission> Permissions { get; set; } = new List<VendorPermission>();
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
