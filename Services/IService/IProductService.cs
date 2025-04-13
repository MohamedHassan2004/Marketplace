using Marketplace.DAL.Models;

namespace Marketplace.Services.IService
{
    public interface IProductService
    {
        Task<bool> AddProduct(Product product);
        Task<bool> UpdateProduct(Product product);
        Task<bool> DeleteProduct(string productId);
        Task<Product> GetProductById(string productId);
        Task<IEnumerable<Product>> GetAllProducts();
        Task<IEnumerable<Product>> GetAcceptedProducts();
        Task<IEnumerable<Product>> GetRejectedProducts();
        Task<IEnumerable<Product>> GetWaitingProducts();
        Task<IEnumerable<Product>> GetProductsByVendorId(string vendorId);
        Task<IEnumerable<Product>> GetProductsByPriceRange(decimal minPrice, decimal maxPrice);
        Task<IEnumerable<Product>> GetProductsByCategoryName(string categoryName);
    }
}
