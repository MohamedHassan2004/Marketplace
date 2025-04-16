using Marketplace.DAL.Models;

namespace Marketplace.DAL.IRepository
{
    public interface ISavedProductRepository
    {
        Task<IEnumerable<SavedProduct>> GetSavedProductsByCustomerIdAsync(string CustomerId);
        Task<bool> SaveProductAsync(SavedProduct savedProduct);
        Task<bool> UnsaveProductAsync(int Id);
    }
}
