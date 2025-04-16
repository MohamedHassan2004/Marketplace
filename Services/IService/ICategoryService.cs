using Marketplace.DAL.Models;
using Marketplace.Services.DTOs;
using Marketplace.Services.DTOs.Product;

namespace Marketplace.Services.IService
{
    public interface ICategoryService
    {
        Task<bool> AddCategoryAsync(CategoryDto category);
        Task<bool> UpdateCategoryAsync(CategoryDto category);
        Task<bool> DeleteCategoryAsync(int id);
        Task<CategoryDto> GetCategoryByIdAsync(int id);
        Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync();
    }
}
