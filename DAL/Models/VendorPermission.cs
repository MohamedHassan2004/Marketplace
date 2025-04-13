using System.ComponentModel.DataAnnotations.Schema;
using Marketplace.DAL.Models.Users;

namespace Marketplace.DAL.Models
{
    public class VendorPermission
    {
        public int Id { get; set; }
        public string VendorId { get; set; }
        public string AdminId { get; set; }
        public int PermissionId { get; set; }
        public DateTime AssignedDate { get; set; }
        [ForeignKey("VendorId")]
        public virtual Vendor Vendor { get; set; }
        [ForeignKey("AdminId")]
        public virtual Admin Admin { get; set; }
        [ForeignKey("PermissionId")]
        public virtual Permission Permission { get; set; }
    }
}
