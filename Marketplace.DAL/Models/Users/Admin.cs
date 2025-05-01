namespace Marketplace.DAL.Models.Users
{
    public class Admin : ApplicationUser
    {
        public ICollection<VendorPermission> Permissions { get; set; } = new List<VendorPermission>();
        public ICollection<Product> WaitingProducts { get; set; } = new List<Product>();
        public ICollection<Vendor> WaitingVendors { get; set; } = new List<Vendor>();
    }
}
