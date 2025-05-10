using Marketplace.DAL.Context;
using Marketplace.DAL.IRepository;
using Marketplace.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.DAL.Repository
{
    public class PermissionRepository : IPermissionRepository
    {
        private readonly MarketplaceDbContext context;

        public PermissionRepository(MarketplaceDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Permission>> GetAllPermissionsAsync()
        {
            return await context.Permissions.AsNoTracking().ToListAsync();
        }

        public async Task<Permission> GetPermissionByIdAsync(int id)
        {
            return await context.Permissions
                .Include(p => p.VendorPermissions)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}
