using Marketplace.DAL.Enums;
using Marketplace.DAL.Models;
using Marketplace.Services.DTOs.Product;

namespace Marketplace.Services.IService
{
    public interface IProductService
    {

        // Customer 
        Task<IEnumerable<AcceptedProductDto>> GetAcceptedProductsAsync(string? customerId);
        Task<IEnumerable<AcceptedProductDto>> GetAcceptedProductsByVendorIdAsync(string? customerId,string vendorId);
        Task<IEnumerable<AcceptedProductDto>> GetProductsByPriceRangeAsync(string? customerId, decimal minPrice, decimal maxPrice);
        Task<IEnumerable<AcceptedProductDto>> GetProductsByCategoryNameAsync(string? customerId, string categoryName);
        Task<IEnumerable<AcceptedProductDto>> GetProductsByCategoryIdAsync(string? customerId, int categoryId);
        Task<AcceptedProductDto> GetAcceptedProductByIdAsync(string? customerId, int productId);


        // Admin
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<IEnumerable<RejectedProductDto>> GetRejectedProductsAsync();
        Task<IEnumerable<WaitingProductDto>> GetWaitingProductsAsync();
        Task<bool> UpdateProductStatusAsync(ProductStatusUpdateDto statusDto);


        // Vendor
        Task<bool> AddProductAsync(ProductCreateDto product, string vendorId);
        Task<bool> UpdateProductAsync(ProductCreateDto product);


        // Vendor + Admin
        Task<bool> DeleteProductAsync(int productId);
        Task<IEnumerable<Product>> GetAllProductsByVendorIdAsync(string vendorId);
        Task<IEnumerable<RejectedProductDto>> GetRejectedProductsByVendorIdAsync(string vendorId);
        Task<IEnumerable<WaitingProductDto>> GetWaitingProductsByVendorIdAsync(string vendorId);
            // accepted , pending , rejected
        Task<Product> GetProductByIdAsync(int productId);

        // auto
        Task<bool> IncreamentViewsNumber(int productId);

    }
}
