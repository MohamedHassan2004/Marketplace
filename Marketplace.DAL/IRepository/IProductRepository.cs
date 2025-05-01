using Marketplace.DAL.Models;

namespace Marketplace.DAL.IRepository
{
    public interface IProductRepository : IRepo<Product>
    {

        // Customer + Vendor + Admin
        Task<Product> GetProductByIdAsync(int productId);
        Task<IEnumerable<Product>> GetAcceptedProductsAsync();
        Task<IEnumerable<Product>> GetProductsByProductNameOrCategoryNameAsync(string Name); 
        Task<IEnumerable<Product>> GetAcceptedProductsByVendorIdAsync(string vendorId);
        Task<IEnumerable<Product>> GetProductsByPriceRangeAsync(decimal minPrice, decimal maxPrice);
        Task<IEnumerable<Product>> GetProductsByCategoryIdAsync(int categoryId);

        // Admin
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<IEnumerable<Product>> GetRejectedProductsAsync();
        Task<IEnumerable<Product>> GetWaitingProductsAsync();

        // Vendor + Admin
        Task<IEnumerable<Product>> GetAllProductsByVendorIdAsync(string vendorId);
        Task<IEnumerable<Product>> GetRejectedProductsByVendorIdAsync(string vendorId);
        Task<IEnumerable<Product>> GetWaitingProductsByVendorIdAsync(string vendorId);
    }
}
