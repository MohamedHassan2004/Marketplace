using Marketplace.DAL.Enums;
using Marketplace.DAL.Models;
using Marketplace.Services.DTOs.Product;
using Microsoft.AspNetCore.Http;

namespace Marketplace.Services.IService
{
    public interface IProductService
    {

        // Customer + admin + vendor
        Task<IEnumerable<ProductWithSavedDto>> GetAcceptedProductsAsync(string? role, string? userId);
        Task<IEnumerable<ProductWithSavedDto>> GetAcceptedProductsByVendorIdAsync(string? role, string? userId, string vendorId);
        Task<IEnumerable<ProductWithSavedDto>> GetProductsByProductNameOrCategoryNameAsync(string? role, string? userId, string Name);
        Task<IEnumerable<ProductWithSavedDto>> GetProductsByCategoryIdAsync(string? role, string? userId, int categoryId);
        Task<IEnumerable<ProductWithSavedDto>> GetProductsByPriceRangeAsync(string? role, string? userId, decimal minPrice, decimal maxPrice);
        Task<ProductWithSavedDto> GetProductByIdAsync(string? role, string? userId, int productId);


        // Vendor + Admin
        Task<bool> DeleteProductAsync(int productId);
        Task<IEnumerable<ProductDto>> GetAllProductsByVendorIdAsync(string userId, string vendorId);
        Task<IEnumerable<ProductDto>> GetRejectedProductsByVendorIdAsync(string userId, string vendorId);
        Task<IEnumerable<ProductDto>> GetWaitingProductsByVendorIdAsync(string userId, string vendorId);


        // Admin
        Task<IEnumerable<ProductDto>> GetAllProductsAsync(string userId);
        Task<IEnumerable<ProductDto>> GetRejectedProductsAsync(string userId);
        Task<IEnumerable<ProductDto>> GetWaitingProductsAsync(string userId);
        Task<bool> UpdateProductStatusAsync(int productId, ProductStatusUpdateDto statusDto);


        // Vendor
        Task<bool> AddProductAsync(ProductCreateDto product, string vendorId);
        Task<bool> UpdateProductAsync(int Id, ProductUpdateDto product);
        Task<bool> UpdateQuantityAsync(int productId, int quantity);
        Task<bool> UpdateProductImgAsync(int id, IFormFile newImg);
        Task<IEnumerable<ProductHistoryDto>> GetProductHistoryAsync(int productId);



        // auto
        Task<bool> IncreamentViewsNumber(int productId);

    }
}
