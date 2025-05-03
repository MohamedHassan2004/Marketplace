using Marketplace.DAL.Models;
using Marketplace.DAL.Models.Users;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.DAL.Context
{
    public class MarketplaceDbContext : IdentityDbContext<ApplicationUser>
    {
        public MarketplaceDbContext(DbContextOptions<MarketplaceDbContext> options) : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<SavedProduct> SavedProducts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<VendorPermission> VendorPermissions { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // VendorPermission
            modelBuilder.Entity<VendorPermission>()
                .HasOne(vp => vp.Vendor)
                .WithMany(v => v.Permissions)
                .HasForeignKey(vp => vp.VendorId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<VendorPermission>()
                .HasOne(vp => vp.Admin)
                .WithMany(a => a.Permissions)
                .HasForeignKey(vp => vp.AdminId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<VendorPermission>()
                .HasOne(vp => vp.Permission)
                .WithMany(p => p.VendorPermissions)
                .HasForeignKey(vp => vp.PermissionId)
                .OnDelete(DeleteBehavior.Cascade);

            // Saved Product
            modelBuilder.Entity<SavedProduct>()
                .HasOne(sp => sp.Customer)
                .WithMany(c => c.SavedProducts)
                .HasForeignKey(sp => sp.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SavedProduct>()
                .HasOne(sp => sp.Product)
                .WithMany(p => p.SavedProducts)
                .HasForeignKey(sp => sp.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            // products
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Vendor)
                .WithMany(v => v.Products)
                .HasForeignKey(p => p.VendorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.AdminChecked)
                .WithMany(a => a.WaitingProducts)
                .HasForeignKey(p => p.AdminCheckedId)
                .OnDelete(DeleteBehavior.NoAction);

            // Order
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Customer)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);


            // OrderItem
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Product)
                .WithMany()
                .HasForeignKey(oi => oi.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Order>()
                .Property(o => o.ShippingCost)
                .HasPrecision(18, 2);
        }
    }
}
