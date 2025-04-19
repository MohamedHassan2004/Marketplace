using Marketplace.DAL.Enums;
using Marketplace.DAL.Models;
using Marketplace.Services.DTOs.Product;

namespace Marketplace.Services.IService
{
    public interface IProductService
    {

        // Customer 
        Task<IEnumerable<ProductDto>> GetAcceptedProductsAsync(string? customerId);
        Task<IEnumerable<ProductDto>> GetAcceptedProductsByVendorIdAsync(string? customerId,string vendorId);
        Task<IEnumerable<ProductDto>> GetProductsByPriceRangeAsync(string? customerId, decimal minPrice, decimal maxPrice);
        Task<IEnumerable<ProductDto>> GetProductsByCategoryNameAsync(string? customerId, string categoryName);
        Task<IEnumerable<ProductDto>> GetProductsByCategoryIdAsync(string? customerId, int categoryId);
        Task<ProductDto> GetAcceptedProductByIdAsync(string? customerId, int productId);


        // Admin
        Task<IEnumerable<ProductDto>> GetAllProductsAsync();
        Task<IEnumerable<ProductDto>> GetRejectedProductsAsync();
        Task<IEnumerable<ProductDto>> GetWaitingProductsAsync();
        Task<bool> UpdateProductStatusAsync(ProductStatusUpdateDto statusDto);


        // Vendor
        Task<bool> AddProductAsync(ProductCreateDto product, string vendorId);
        Task<bool> UpdateProductAsync(int Id, ProductCreateDto product);


        // Vendor + Admin
        Task<bool> DeleteProductAsync(int productId);
        Task<IEnumerable<ProductDto>> GetAllProductsByVendorIdAsync(string vendorId);
        Task<IEnumerable<ProductDto>> GetRejectedProductsByVendorIdAsync(string vendorId);
        Task<IEnumerable<ProductDto>> GetWaitingProductsByVendorIdAsync(string vendorId);
            // accepted , pending , rejected
        Task<Product> GetProductByIdAsync(int productId);

        // auto
        Task<bool> IncreamentViewsNumber(int productId);

    }
}
