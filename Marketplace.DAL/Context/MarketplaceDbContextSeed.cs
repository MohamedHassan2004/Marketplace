using Marketplace.DAL.Enums;
using Marketplace.DAL.Models;
using Marketplace.DAL.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.DAL.Context
{
    public class MarketplaceDbContextSeed
    {
        public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            var roles = new[] { "Admin", "Vendor", "Customer" };

            foreach (var role in roles)
            {
                var roleExist = await roleManager.RoleExistsAsync(role);
                if (!roleExist)
                {
                    var identityRole = new IdentityRole(role);
                    await roleManager.CreateAsync(identityRole);
                }
            }
        }

        public static async Task SeedCategoriesAsync(MarketplaceDbContext context)
        {
            if (!context.Categories.Any())
            {
                var categories = new List<Category>
                {
                    new Category { Name = "Electronics", ImgUrl = "images/categories/539ff381-0dc9-498c-82a6-8086f0e32d23.jpg" },
                    new Category { Name = "Clothing", ImgUrl = "images/categories/clothing.jpg" },
                    new Category { Name = "Home Furniture", ImgUrl = "images/categories/home_furniture.jpg" },
                    new Category { Name = "Books", ImgUrl = "images/categories/books.jpg" },
                    new Category { Name = "Toys", ImgUrl = "images/categories/toys.jpg" },
                    new Category { Name = "Sports Equipment", ImgUrl = "images/categories/sports_equipment.jpg" },
                    new Category { Name = "Groceries", ImgUrl = "images/categories/groceries.jpg" },
                    new Category { Name = "Beauty & Personal Care", ImgUrl = "images/categories/beauty_personal_care.jpg" }
                };

                await context.Categories.AddRangeAsync(categories);
                await context.SaveChangesAsync();
            }
        }

        public static async Task SeedPermissionsAsync(MarketplaceDbContext context)
        {

            var existingPermissions = await context.Permissions.Select(p => p.Id).ToListAsync();
            var newPermissions = Enum.GetValues(typeof(Permissions))
                .Cast<Permissions>()
                .Where(permission => !existingPermissions.Contains((int)permission))
                .Select(permission => new Permission
                {
                    Name = permission.ToString()
                });

            if (newPermissions.Any())
            {
                await context.Permissions.AddRangeAsync(newPermissions);
                await context.SaveChangesAsync();
            }
        }
    }
}
