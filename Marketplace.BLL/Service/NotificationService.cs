using AutoMapper;
using Marketplace.BLL.DTOs;
using Marketplace.BLL.IService;
using Marketplace.DAL.IRepository;
using Marketplace.DAL.Models;
using Marketplace.DAL.Models.Users;
using Marketplace.DAL.WebSockets;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.BLL.Service
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IVendorRepository _vendorRepository;
        private readonly IMapper _mapper;
        private readonly WebSocketConnectionManager _webSocketConnectionManager;

        public NotificationService(INotificationRepository notificationRepository, WebSocketConnectionManager webSocketConnectionManager, IMapper mapper, IVendorRepository vendorRepository)
        {
            _notificationRepository = notificationRepository;
            _webSocketConnectionManager = webSocketConnectionManager;
            _mapper = mapper;
            _vendorRepository = vendorRepository;
        }

        public async Task<bool> SendNotificationAsync(string userId, string message)
        {
            var notification = new Notification
            {
                UserId = userId,
                Message = message,
                CreatedAt = DateTime.UtcNow
            };
            await _notificationRepository.AddAsync(notification);

            // Send notification via WebSocket
            var socket = _webSocketConnectionManager.GetSocketByVendorId(userId);
            if (socket != null && socket.State == WebSocketState.Open)
            {
                var notificationMessage = Encoding.UTF8.GetBytes(message);
                await socket.SendAsync(new ArraySegment<byte>(notificationMessage), WebSocketMessageType.Text, true, CancellationToken.None);
                return true;
            }
            else
            {
                return false;
            }
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
            foreach (var notification in NotificationsEntities)
            {
                notification.IsRead = true;
                await _notificationRepository.UpdateAsync(notification);
            }
            var dtos = _mapper.Map<IEnumerable<NotificationDto>>(NotificationsEntities);
            return dtos;
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
