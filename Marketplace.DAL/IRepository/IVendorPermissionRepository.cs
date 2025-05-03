using Marketplace.DAL.Models;
using Marketplace.DAL.Models.Users;

namespace Marketplace.DAL.IRepository
{
    public interface IVendorPermissionRepository
    {
        Task<IEnumerable<VendorPermission>> GetVendorWithPermissionsDetailsAsync(string vendorId);
        Task<bool> AssignPermissionToVendorAsync(string vendorId, int permissionId, string adminId);
        Task<bool> RemovePermissionFromVendorAsync(VendorPermission vendorPermission);
        Task<VendorPermission> GetVendorPermissionByIdAsync(int Id);
    }
}
