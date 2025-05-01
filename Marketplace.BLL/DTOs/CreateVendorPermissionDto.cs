using System.ComponentModel.DataAnnotations;

namespace Marketplace.Services.DTOs
{
    public class CreateVendorPermissionDto
    {
        [Required]
        public string VendorId { get; set; }
        [Required]
        public int PermissionId { get; set; }
    }
}
