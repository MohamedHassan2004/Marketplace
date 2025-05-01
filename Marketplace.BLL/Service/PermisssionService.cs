using AutoMapper;
using Marketplace.DAL.IRepository;
using Marketplace.Services.DTOs;
using Marketplace.Services.IService;

namespace Marketplace.Services.Service
{
    public class PermisssionService : IPermisssionService
    {
        private readonly IPermissionRepository _permissionRepository;
        private readonly IMapper _mapper;

        public PermisssionService(IPermissionRepository permissionRepository, IMapper mapper)
        {
            _permissionRepository = permissionRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PermissionDto>> GetAllPermissionsAsync()
        {
            var permissionEntities = await _permissionRepository.GetAllPermissionsAsync();
            var permissionDtos = _mapper.Map<IEnumerable<PermissionDto>>(permissionEntities);
            return permissionDtos;
        }

        public async Task<PermissionDto> GetPermissionByIdAsync(int id)
        {
            var permission = await _permissionRepository.GetPermissionByIdAsync(id);
            if (permission == null)
            {
                return null;
            }
            return _mapper.Map<PermissionDto>(permission);
        }
    }
}
