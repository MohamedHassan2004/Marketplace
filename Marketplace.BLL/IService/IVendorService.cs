using Marketplace.DAL.Enums;
using Marketplace.DAL.Models.Users;
using Marketplace.Services.DTOs.Vendor;

namespace Marketplace.Services.IService
{
    public interface IVendorService
    {
        Task<IEnumerable<VendorDto>> GetAllVendorsAsync();
        Task<IEnumerable<VendorDto>> GetWaitingVendorsAsync();
        Task<VendorDto> GetVendorByIdAsync(string vendorId);
        Task<bool> UpdateVendorStatusAsync(string vendorId ,VendorStatusUpdateDto model, string adminId);
    }
}
