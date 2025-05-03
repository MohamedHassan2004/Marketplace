using Marketplace.BLL.IService;
using Marketplace.BLL.Service;
using Marketplace.DAL.IRepository;
using Marketplace.DAL.Repository;
using Marketplace.DAL.WebSockets;
using Marketplace.Filters;
using Marketplace.Middlewares;
using Marketplace.Services.IService;
using Marketplace.Services.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Marketplace.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMarketplaceServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthorizationHandler, HasPermissionHandler>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IPermissionRepository, PermissionRepository>();
            services.AddScoped<ISavedProductRepository, SavedProductRepository>();
            services.AddScoped<IVendorPermissionRepository, VendorPermissionRepository>();
            services.AddScoped<IVendorRepository, VendorRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderItemRepository, OrderItemRepository>();
            services.AddScoped<INotificationRepository, NotificationRepository>();

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ISavedProductService, SavedProductService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IPermisssionService, PermisssionService>();
            services.AddScoped<IVendorPermissionService, VendorPermissionService>();
            services.AddScoped<IVendorService, VendorService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IOrderItemService, OrderItemService>();
            services.AddScoped<INotificationService, NotificationService>();

            // Register WebSocketConnectionManager as a singleton  
            services.AddSingleton<WebSocketConnectionManager>();

            return services;
        }
    }
}
