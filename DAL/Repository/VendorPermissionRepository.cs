using Marketplace.DAL.Context;
using Marketplace.DAL.IRepository;
using Marketplace.DAL.Models;
using Socialify.DAL.Repository;

namespace Marketplace.DAL.Repository
{
    public class VendorPermissionRepository : Repo<VendorPermission>, IVendorPermissionRepository
    {
        private readonly MarketplaceDbContext _dbContext;
        public VendorPermissionRepository(MarketplaceDbContext context) : base(context)
        {
            _dbContext = context;
        }
    }
}
