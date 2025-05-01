using Marketplace.DAL.Models;

namespace Marketplace.DAL.IRepository
{
    public interface IPermissionRepository
    {
        Task<IEnumerable<Permission>> GetAllPermissionsAsync();
        Task<Permission> GetPermissionByIdAsync(int id);
    }
}
