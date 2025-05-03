using AutoMapper;
using Marketplace.BLL.DTOs;
using Marketplace.BLL.IService;
using Marketplace.DAL.IRepository;
using Marketplace.DAL.Models;
using Marketplace.DAL.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.BLL.Service
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IVendorRepository _vendorRepository;
        private readonly IMapper _mapper;

        public NotificationService(INotificationRepository notificationRepository, IVendorRepository vendorRepository, IMapper mapper)
        {
            _notificationRepository = notificationRepository;
            _vendorRepository = vendorRepository;
            _mapper = mapper;
        }

        public async Task<bool> DeleteNotificationAsync(int notificationId)
        {
            var notification = await _notificationRepository.GetNotificationByIdAsync(notificationId);
            if (notification == null)
            {
                return false;
            }
            await _notificationRepository.DeleteAsync(notification);
            return true;
        }

        public async Task<IEnumerable<NotificationDto>> GetNotificationsByUserIdAsync(string userId)
        {
            var NotificationsEntities = await _notificationRepository.GetUserNotificationsAsync(userId);
            if (NotificationsEntities == null || !NotificationsEntities.Any())
            {
                return Enumerable.Empty<NotificationDto>();
            }
            var dtos = _mapper.Map<IEnumerable<NotificationDto>>(NotificationsEntities);
            return dtos;
        }

        public async Task<bool> MarkNotificationAsReadAsync(int notificationId)
        {
            var notification = await _notificationRepository.GetNotificationByIdAsync(notificationId);
            if (notification == null)
            {
                return false;
            }
            notification.IsRead = true;
            await _notificationRepository.UpdateAsync(notification);
            return true;
        }

        public async Task<bool> SendNotificationAsync(string userId, string message)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(message))
            {
                return false;
            }
            var notification = new Notification
            {
                UserId = userId,
                Message = message,
                CreatedAt = DateTime.UtcNow,
                IsRead = false
            };
            await _notificationRepository.AddAsync(notification);
            return true;
        }

        public async Task<bool> SendNotificationToAllVendorsAsync(string message)
        {
            var vendors = await _vendorRepository.GetAllVendorsAsync();
            if (vendors == null || !vendors.Any())
            {
                return false;
            }
            foreach (var vendor in vendors)
            {
                var notification = new Notification
                {
                    UserId = vendor.Id,
                    Message = message,
                    CreatedAt = DateTime.UtcNow,
                    IsRead = false
                };
                await _notificationRepository.AddAsync(notification);
            }
            return true;
        }
    }
}
