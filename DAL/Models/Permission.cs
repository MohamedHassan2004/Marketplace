using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace Marketplace.DAL.Models
{
    public class Permission
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<VendorPermission> VendorPermissions { get; set; } = new List<VendorPermission>();
    }
}
