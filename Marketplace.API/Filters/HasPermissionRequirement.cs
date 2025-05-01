using Microsoft.AspNetCore.Authorization;

namespace Marketplace.Filters
{
    public class HasPermissionRequirement : IAuthorizationRequirement
    {
        public int PermissionId { get; }

        public HasPermissionRequirement(int permissionId)
        {
            PermissionId = permissionId;
        }
    }
}
