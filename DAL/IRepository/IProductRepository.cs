using Marketplace.DAL.Models;

namespace Marketplace.DAL.IRepository
{
    public interface IProductRepository : IRepo<Product>
    {

        // Customer + Vendor + Admin
        Task<IEnumerable<Product>> GetAcceptedProductsAsync();
        Task<IEnumerable<Product>> GetAcceptedProductsByVendorIdAsync(string vendorId);
        Task<IEnumerable<Product>> GetProductsByPriceRangeAsync(decimal minPrice, decimal maxPrice);
        Task<IEnumerable<Product>> GetProductsByCategoryNameAsync(string categoryName);
        Task<IEnumerable<Product>> GetProductsByCategoryIdAsync(int categoryId);

        Task<Product> GetAcceptedProductByIdAsync(int productId);

        // Admin
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<IEnumerable<Product>> GetRejectedProductsAsync();
        Task<IEnumerable<Product>> GetWaitingProductsAsync();

        // Vendor + Admin
        Task<IEnumerable<Product>> GetAllProductsByVendorIdAsync(string vendorId);
        Task<IEnumerable<Product>> GetRejectedProductsByVendorIdAsync(string vendorId);
        Task<IEnumerable<Product>> GetWaitingProductsByVendorIdAsync(string vendorId);
        Task<Product> GetProductByIdAsync(int productId);
    }
}
