using System.ComponentModel.DataAnnotations.Schema;
using Marketplace.DAL.Models.Users;

namespace Marketplace.DAL.Models
{
    public class VendorPermission
    {
        public int Id { get; set; }
        public required string VendorId { get; set; }
        public string AdminId { get; set; }
        public int PermissionId { get; set; }
        public DateTime AssignedDate { get; set; }
        public Vendor Vendor { get; set; }
        public Admin Admin { get; set; }
        public Permission Permission { get; set; }
    }
}
