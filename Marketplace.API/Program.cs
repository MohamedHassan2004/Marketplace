using AutoMapper;
using Marketplace.API.Hubs;
using Marketplace.BLL.IService;
using Marketplace.BLL.Service;
using Marketplace.DAL.Context;
using Marketplace.DAL.IRepository;
using Marketplace.DAL.Models.Users;
using Marketplace.DAL.Repository;
using Marketplace.Filters;
using Marketplace.Middlewares;
using Marketplace.Services.AutoMapper;
using Marketplace.Services.IService;
using Marketplace.Services.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace Marketplace
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<MarketplaceDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 8;

                options.User.RequireUniqueEmail = true;

                options.Lockout.MaxFailedAccessAttempts = 3;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(2);
            })
            .AddEntityFrameworkStores<MarketplaceDbContext>();


            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
                        RoleClaimType = ClaimTypes.Role
                    };
                });

            builder.Services.AddAuthorization(options =>
            {
                using var scope = builder.Services.BuildServiceProvider().CreateScope();
                var permissionRepository = scope.ServiceProvider.GetRequiredService<IPermissionRepository>();
                var permissions = permissionRepository.GetAllPermissionsAsync().GetAwaiter().GetResult();

                foreach (var permission in permissions)
                {
                    options.AddPolicy($"Permission:{permission.Id}",
                        policy => policy.Requirements.Add(new HasPermissionRequirement(permission.Id)));
                }
            });


            builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

            builder.Services.AddScoped<IAuthorizationHandler, HasPermissionHandler>();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<IPermissionRepository, PermissionRepository>();
            builder.Services.AddScoped<ISavedProductRepository, SavedProductRepository>();
            builder.Services.AddScoped<IVendorPermissionRepository, VendorPermissionRepository>();
            builder.Services.AddScoped<IVendorRepository, VendorRepository>();
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();
            builder.Services.AddScoped<IOrderItemRepository, OrderItemRepository>();
            builder.Services.AddScoped<INotificationRepository, NotificationRepository>();

            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<ISavedProductService, SavedProductService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IPermisssionService, PermisssionService>();
            builder.Services.AddScoped<IVendorPermissionService, VendorPermissionService>();
            builder.Services.AddScoped<IVendorService, VendorService>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<IOrderItemService, OrderItemService>();
            builder.Services.AddScoped<INotificationService, NotificationService>();

            builder.Services.AddSignalR();



            // CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowReactApp",
                    policy =>
                    {
                        policy.WithOrigins("http://localhost:5173")
                              .AllowAnyMethod()
                              .AllowAnyHeader();
                    });
            });


            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();


            // seed data in database
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                // Role seeding
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                MarketplaceDbContextSeed.SeedRolesAsync(roleManager).Wait();

                // Data seeding
                var context = services.GetRequiredService<MarketplaceDbContext>();

                MarketplaceDbContextSeed.SeedCategoriesAsync(context).Wait();
                MarketplaceDbContextSeed.SeedPermissionsAsync(context).Wait();
            }


            // Cors
            app.UseCors("AllowReactApp");
            app.UseStaticFiles();


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.MapHub<NotificationHub>("/notificationHub");

            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
