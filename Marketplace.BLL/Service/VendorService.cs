using AutoMapper;
using Marketplace.DAL.Enums;
using Marketplace.DAL.IRepository;
using Marketplace.DAL.Repository;
using Marketplace.Services.DTOs.Vendor;
using Marketplace.Services.IService;

namespace Marketplace.Services.Service
{
    public class VendorService : IVendorService
    {
        private readonly IVendorRepository _vendorRepository;
        private readonly IMapper _mapper;

        public VendorService(IVendorRepository vendorRepository, IMapper mapper)
        {
            _vendorRepository = vendorRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<VendorDto>> GetAllVendorsAsync()
        {
            var entities = await _vendorRepository.GetAllVendorsAsync();
            var dtos = _mapper.Map<IEnumerable<VendorDto>>(entities);
            return dtos;
        }

        public async Task<VendorDto> GetVendorByIdAsync(string vendorId)
        {
            var entity = await _vendorRepository.GetVendorByIdAsync(vendorId);
            var dto = _mapper.Map<VendorDto>(entity);
            return dto;
        }

        public async Task<IEnumerable<VendorDto>> GetWaitingVendorsAsync()
        {
            var entities = await _vendorRepository.GetWaitingVendorsAsync();
            var dtos = _mapper.Map<IEnumerable<VendorDto>>(entities);
            return dtos;
        }

        public async Task<bool> UpdateVendorStatusAsync(string vendorId, VendorStatusUpdateDto model, string adminId)
        {
            var vendor = await _vendorRepository.GetVendorByIdAsync(vendorId);
            if (vendor == null)
            {
                return false;
            }
            vendor.ApprovalStatus = model.Status;
            vendor.RejectionReason = model.RejectionReason;
            vendor.AdminIdChecked = adminId;
            return await _vendorRepository.UpdateVendorAsync(vendor);
        }
    }
}
