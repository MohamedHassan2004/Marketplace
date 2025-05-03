using Marketplace.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.DAL.IRepository
{
    public interface INotificationRepository
    {
        Task AddAsync(Notification notification);
        Task<IEnumerable<Notification>> GetUserNotificationsAsync(string userId);
        Task UpdateAsync(Notification notification);
        Task DeleteAsync(Notification notification);
        Task<Notification> GetNotificationByIdAsync(int notificationId);
    }
}
