using Marketplace.DAL.IRepository;
using Marketplace.DAL.Models;
using Marketplace.DAL.Repository;
using Marketplace.Services.DTOs;
using Marketplace.Services.IService;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Marketplace.Services.Service
{
    public class VendorPermissionService : IVendorPermissionService
    {
        private readonly IVendorPermissionRepository _vendorPermissionRepository;
        private readonly IPermissionRepository _permissionRepository;

        public VendorPermissionService(IVendorPermissionRepository vendorPermissionRepository, IPermissionRepository permissionRepository)
        {
            _vendorPermissionRepository = vendorPermissionRepository;
            _permissionRepository = permissionRepository;
        }


        public async Task<bool> AssignPermissionToVendorAsync(CreateVendorPermissionDto vendorPermissionDto, string adminId)
        {
            var permissionExists = await _permissionRepository.GetPermissionByIdAsync(vendorPermissionDto.PermissionId);
            if (permissionExists == null)
                return false;

            return await _vendorPermissionRepository.AssignPermissionToVendorAsync(vendorPermissionDto.VendorId, vendorPermissionDto.PermissionId, adminId);
        }

        public async Task<VendorPermissionDto> GetVendorPermissionByIdAsync(int id)
        {
            var vendorPermission = await _vendorPermissionRepository.GetVendorPermissionByIdAsync(id);
            var dto = new VendorPermissionDto
            {
                VendorId = vendorPermission.VendorId,
                AdminId = vendorPermission.AdminId,
                PermissionId = vendorPermission.PermissionId,
                AssignedDate = vendorPermission.AssignedDate
            };
            return dto; 
        }

        public async Task<IEnumerable<VendorPermissionDto>> GetVendorWithPermissionsDetailsAsync(string vendorId)
        {
            var vendorPermissions = await _vendorPermissionRepository.GetVendorWithPermissionsDetailsAsync(vendorId);

            if (vendorPermissions == null || !vendorPermissions.Any())
                return Enumerable.Empty<VendorPermissionDto>();

            return vendorPermissions.Select(vp => new VendorPermissionDto
            {
                VendorId = vp.VendorId,
                AdminId = vp.AdminId,
                PermissionId = vp.PermissionId,
                AssignedDate = vp.AssignedDate
            });
        }

        public async Task<bool> RemovePermissionFromVendorAsync(int id)
        {
            var vendorPermission = await _vendorPermissionRepository.GetVendorPermissionByIdAsync(id);
            if(vendorPermission == null) return false;
            return await _vendorPermissionRepository.RemovePermissionFromVendorAsync(vendorPermission);
        }
    }
}
