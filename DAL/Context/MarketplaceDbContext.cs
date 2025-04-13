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
        public DbSet<ProductReview> ProductReviews { get; set; }
        public DbSet<SavedProduct> SavedProducts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<VendorPermission> VendorPermissions { get; set; }
        public DbSet<Permission> Permissions { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // VendorPermission
            modelBuilder.Entity<VendorPermission>()
            .HasOne(vp => vp.Vendor)
            .WithMany()
            .HasForeignKey(vp => vp.VendorId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<VendorPermission>()
                .HasOne(vp => vp.Admin)
                .WithMany()
                .HasForeignKey(vp => vp.AdminId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<VendorPermission>()
                .HasOne(vp => vp.Permission)
                .WithMany(p => p.VendorPermissions)
                .HasForeignKey(vp => vp.PermissionId)
                .OnDelete(DeleteBehavior.Cascade);

            // Order
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Customer)
                .WithMany()
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Vendor)
                .WithMany()
                .HasForeignKey(o => o.VendorId)
                .OnDelete(DeleteBehavior.Restrict);

            // Product Review 
            modelBuilder.Entity<ProductReview>()
                .HasOne(pr => pr.Customer)
                .WithMany()
                .HasForeignKey(pr => pr.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ProductReview>()
                .HasOne(pr => pr.Product)
                .WithMany(p => p.Reviews)
                .HasForeignKey(pr => pr.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // Saved Product
            modelBuilder.Entity<SavedProduct>()
                .HasOne(sp => sp.Customer)
                .WithMany()
                .HasForeignKey(sp => sp.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SavedProduct>()
                .HasOne(sp => sp.Product)
                .WithMany(p => p.SavedProducts)
                .HasForeignKey(sp => sp.ProductId)
                .OnDelete(DeleteBehavior.Restrict);



        }
    }
}
