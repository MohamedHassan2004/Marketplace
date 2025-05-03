using Microsoft.AspNetCore.Identity;

namespace Marketplace.DAL.Models.Users
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    }
}
