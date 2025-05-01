using Marketplace.Services.DTOs.Product;

namespace Marketplace.Services.IService
{
    public interface ISavedProductService
    {
        Task<bool> SaveProduct(int productId, string customerId);
        Task<bool> UnsaveProduct(int SaveId);
        Task<IEnumerable<SavedProductDto>> GetSavedProductsAsync(string customerId);
    }
}
