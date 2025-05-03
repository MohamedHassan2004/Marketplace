using Marketplace.BLL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.BLL.IService
{
    public interface INotificationService
    {
        Task<bool> SendNotificationAsync(string userId, string message);
        Task<IEnumerable<NotificationDto>> GetNotificationsByUserIdAsync(string userId);
        Task<bool> MarkNotificationAsReadAsync(int notificationId);
        Task<bool> DeleteNotificationAsync(int notificationId);
        Task<bool> SendNotificationToAllVendorsAsync(string message);
    }
}
