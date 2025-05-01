using Marketplace.DAL.Enums;
using Microsoft.AspNetCore.Authorization;

namespace Marketplace.Filters
{
    public class RequirePermissionAttribute : AuthorizeAttribute
    {
        public RequirePermissionAttribute(Permissions permission)
        {
            Policy = $"Permission:{(int)permission}";
        }
    }
}