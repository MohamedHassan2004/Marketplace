using Marketplace.DAL.Models;

namespace Marketplace.DAL.IRepository
{
    public interface IProductRepository : IRepo<Product>
    {
        Task<Product> GetProductWithDetailsById(int Id);
    }
}
