using Marketplace.DAL.Models;
using Marketplace.Services.DTOs.Category;
using Marketplace.Services.DTOs.Product;

namespace Marketplace.Services.IService
{
    public interface ICategoryService
    {
        Task<bool> AddCategoryAsync(CategoryCreateDto category);
        Task<bool> UpdateCategoryAsync(int id, CategoryCreateDto categoryDto);
        Task<bool> DeleteCategoryAsync(int id);
        Task<CategoryDto> GetCategoryByIdAsync(int id);
        Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync();
    }
}
