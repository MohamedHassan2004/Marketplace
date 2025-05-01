using Marketplace.DAL.Context;
using Marketplace.DAL.IRepository;
using Marketplace.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.DAL.Repository
{
    public class VendorPermissionRepository : IVendorPermissionRepository
    {
        private readonly MarketplaceDbContext _dbContext;
        public VendorPermissionRepository(MarketplaceDbContext context)
        {
            _dbContext = context;
        }

        public async Task<bool> AssignPermissionToVendorAsync(string vendorId, int permissionId, string adminId)
        {
            await _dbContext.VendorPermissions.AddAsync(new VendorPermission
            {
                VendorId = vendorId,
                PermissionId = permissionId,
                AdminId = adminId,
                AssignedDate = DateTime.UtcNow
            });
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<VendorPermission>> GetVendorWithPermissionsDetailsAsync(string vendorId)
        {
            return await _dbContext.VendorPermissions
                .Include(vp => vp.Permission)
                .Where(vp => vp.VendorId == vendorId)
                .ToListAsync();
        }

        public async Task<bool> RemovePermissionFromVendorAsync(int Id) 
        { 
            var vendorPermission = await _dbContext.VendorPermissions.FindAsync(Id);
            if (vendorPermission != null)
            {
                _dbContext.VendorPermissions.Remove(vendorPermission);
                return await _dbContext.SaveChangesAsync() > 0;
            }
            return false;
        }
    }
}
