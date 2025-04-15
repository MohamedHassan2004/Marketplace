using Marketplace.DAL.Context;
using Marketplace.DAL.IRepository;
using Marketplace.DAL.Models;

namespace Marketplace.DAL.Repository
{
    public class PermissionRepository : Repo<Permission> , IPermissionRepository
    {
        public PermissionRepository(MarketplaceDbContext context) : base(context)
        {
        }
    }
}
