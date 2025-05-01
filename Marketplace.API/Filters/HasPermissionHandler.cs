using Marketplace.DAL.IRepository;
using Marketplace.DAL.Models.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Marketplace.Filters
{
    public class HasPermissionHandler : AuthorizationHandler<HasPermissionRequirement>
    {
        private readonly IVendorPermissionRepository _vendorPermissionRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public HasPermissionHandler(
            IVendorPermissionRepository vendorPermissionRepository,
            UserManager<ApplicationUser> userManager)
        {
            _vendorPermissionRepository = vendorPermissionRepository;
            _userManager = userManager;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            HasPermissionRequirement requirement)
        {
            if (!context.User.Identity.IsAuthenticated)
            {
                return;
            }

            // If the user is an admin, automatically succeed
            if (context.User.IsInRole("Admin"))
            {
                context.Succeed(requirement);
                return;
            }

            // Check if the user is a vendor
            var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return;
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null || !(user is Vendor))
            {
                return;
            }

            // Get vendor permissions
            var permissions = await _vendorPermissionRepository.GetVendorWithPermissionsDetailsAsync(userId);
            
            // Check if vendor has the required permission
            if (permissions.Any(p => p.PermissionId == requirement.PermissionId))
            {
                context.Succeed(requirement);
            }
        }
    }
}
