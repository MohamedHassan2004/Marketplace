using Marketplace.Services.DTOs;

namespace Marketplace.Services.IService
{
    public interface IVendorPermissionService
    {
        Task<bool> AssignPermissionToVendorAsync(CreateVendorPermissionDto vendorPermissionDto, string adminId);
        Task<bool> RemovePermissionFromVendorAsync(int id);
        Task<IEnumerable<VendorPermissionDto>> GetVendorWithPermissionsDetailsAsync(string vendorId);
        Task<VendorPermissionDto> GetVendorPermissionByIdAsync(int id);
    }
}
