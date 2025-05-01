using Marketplace.Services.DTOs;

namespace Marketplace.Services.IService
{
    public interface IPermisssionService
    {
        Task<IEnumerable<PermissionDto>> GetAllPermissionsAsync();
        Task<PermissionDto> GetPermissionByIdAsync(int id);
    }
}
