using AutoMapper;
using Marketplace.BLL.IService;
using Marketplace.BLL.Service;
using Marketplace.DAL.Context;
using Marketplace.DAL.IRepository;
using Marketplace.DAL.Models.Users;
using Marketplace.DAL.Repository;
using Marketplace.DAL.WebSockets;
using Marketplace.Extensions;
using Marketplace.Filters;
using Marketplace.Middlewares;
using Marketplace.Services.AutoMapper;
using Marketplace.Services.IService;
using Marketplace.Services.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebSockets;
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

            // Use the extension method to add services  
            builder.Services.AddMarketplaceServices();

            builder.Services.AddDataProtection();


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


            // web socket  
            var webSocketOptions = new WebSocketOptions
            {
                KeepAliveInterval = TimeSpan.FromMinutes(2)
            };
            app.UseWebSockets(webSocketOptions);

            app.Use(async (context, next) =>
            {
                if (context.Request.Path == "/ws")
                {
                    if (context.WebSockets.IsWebSocketRequest)
                    {
                        var webSocket = await context.WebSockets.AcceptWebSocketAsync();
                        var connectionManager = context.RequestServices.GetRequiredService<WebSocketConnectionManager>();
                        var vendorId = context.Request.Query["vendorId"]; // Assume vendorId is passed as a query parameter
                        connectionManager.AddConnection(vendorId, webSocket);

                        // Keep the WebSocket connection open
                        await connectionManager.HandleConnectionAsync(vendorId, webSocket);
                    }
                    else
                    {
                        context.Response.StatusCode = 400;
                    }
                }
                else
                {
                    await next();
                }
            });


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

            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
