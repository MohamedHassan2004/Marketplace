namespace Marketplace.Services.DTOs
{
    public class VendorPermissionDto
    {
        public int VendorPermissionId { get; set; }
        public string VendorId { get; set; }
        public string AdminId { get; set; }
        public int PermissionId { get; set; }
        public DateTime AssignedDate { get; set; }
    }
}
