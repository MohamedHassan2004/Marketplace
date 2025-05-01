using Marketplace.DAL.Models.Users;
using System.Threading.Tasks;

namespace Marketplace.DAL.IRepository
{
    public interface IVendorRepository
    {
        Task<bool> UpdateVendorAsync(Vendor vendor);
        Task<IEnumerable<Vendor>> GetAllVendorsAsync();
        Task<IEnumerable<Vendor>> GetWaitingVendorsAsync();
        Task<Vendor> GetVendorByIdAsync(string vendorId);
        
    }
}
