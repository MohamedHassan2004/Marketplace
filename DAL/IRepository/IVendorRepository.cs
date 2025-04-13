using Marketplace.DAL.Models.Users;
using System.Threading.Tasks;

namespace Marketplace.DAL.IRepository
{
    public interface IVendorRepository
    {
        Task<bool> Update(Vendor vendor);
        Task<Vendor> GetVendorWithPermissions(string vendorId);
        Task<IEnumerable<Vendor>> GetAllVendors();
        Task<Vendor> GetVendorById(string vendorId);
    }
}
