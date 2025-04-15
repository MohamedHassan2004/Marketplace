using Marketplace.DAL.Enums;
using Marketplace.DAL.Models;
using Marketplace.Services.DTOs.Product;

namespace Marketplace.Services.IService
{
    public interface IProductService
    {
        // Annonymous

        // Customer 
        Task<IEnumerable<Product>> GetAcceptedProductsAsync();
        Task<IEnumerable<Product>> GetAcceptedProductsByVendorIdAsync(string vendorId);
        Task<IEnumerable<Product>> GetProductsByPriceRangeAsync(decimal minPrice, decimal maxPrice);
        Task<IEnumerable<Product>> GetProductsByCategoryNameAsync(string categoryName);
        Task<Product> GetAcceptedProductByIdAsync(int productId);


        // Admin
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<IEnumerable<Product>> GetRejectedProductsAsync();
        Task<IEnumerable<Product>> GetWaitingProductsAsync();
        Task<bool> UpdateProductStatusAsync(ProductStatusUpdateDto statusDto);


        // Vendor
        Task<bool> AddProductAsync(ProductCreateDto product);
        Task<bool> UpdateProductAsync(Product product);


        // Vendor + Admin
        Task<bool> DeleteProductAsync(int productId);
        Task<IEnumerable<Product>> GetAllProductsByVendorIdAsync(string vendorId);
        Task<IEnumerable<Product>> GetRejectedProductsByVendorIdAsync(string vendorId);
        Task<IEnumerable<Product>> GetWaitingProductsByVendorIdAsync(string vendorId);
        // accepted , pending , rejected
        Task<Product> GetProductByIdAsync(int productId);

        // auto
        Task<bool> IncreamentViewsNumber(int productId);

    }
}
