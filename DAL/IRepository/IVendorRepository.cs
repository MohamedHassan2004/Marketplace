using Marketplace.DAL.Models.Users;
using System.Threading.Tasks;

namespace Marketplace.DAL.IRepository
{
    public interface IVendorRepository
    {
        Task<bool> UpdateVendorAsync(Vendor vendor);
        Task<Vendor> GetVendorWithPermissionsAsync(string vendorId);
        Task<IEnumerable<Vendor>> GetAllVendorsAsync();
        Task<Vendor> GetVendorByIdAsync(string vendorId);
    }
}
