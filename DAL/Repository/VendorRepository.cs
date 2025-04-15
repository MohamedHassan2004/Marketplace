using Marketplace.DAL.Context;
using Marketplace.DAL.Enums;
using Marketplace.DAL.IRepository;
using Marketplace.DAL.Models.Users;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.DAL.Repository
{
    public class VendorRepository : IVendorRepository
    {
        private readonly MarketplaceDbContext _dbContext;

        public VendorRepository(MarketplaceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Vendor>> GetAllVendorsAsync()
        {
            return await _dbContext.Vendors.ToListAsync();
        }

        public async Task<Vendor> GetVendorByIdAsync(string vendorId)
        {
            return await _dbContext.Vendors.FindAsync(vendorId);
        }

        public async Task<Vendor> GetVendorWithPermissionsAsync(string vendorId)
        {
            var vendor = await _dbContext.Vendors
                .Include(v => v.Permissions)
                .FirstOrDefaultAsync(v => v.Id == vendorId);

            return vendor;
        }

        public async Task<bool> UpdateVendorAsync(Vendor vendor)
        {
            _dbContext.Vendors.Update(vendor);
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}
