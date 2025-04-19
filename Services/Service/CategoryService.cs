using AutoMapper;
using Marketplace.DAL.IRepository;
using Marketplace.DAL.Models;
using Marketplace.DAL.Repository;
using Marketplace.Migrations;
using Marketplace.Services.DTOs;
using Marketplace.Services.IService;

namespace Marketplace.Services.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<bool> AddCategoryAsync(CategoryDto category)
        {
            var categoryEntity = _mapper.Map<Category>(category);
            return await _categoryRepository.AddAsync(categoryEntity);
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            return await _categoryRepository.DeleteByIdAsync(id);
        }

        public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
        {
            var entities =  await _categoryRepository.GetAllAsync();
            var dtos = _mapper.Map<IEnumerable<CategoryDto>>(entities);
            return dtos;
        }

        public async Task<CategoryDto> GetCategoryByIdAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category != null) { 
                var dto = _mapper.Map<CategoryDto>(category);
                return dto;
            }
            return null;
        }

        public async Task<bool> UpdateCategoryAsync(int id, CategoryDto categoryDto)
        {
            var existingCategory = await _categoryRepository.GetByIdAsync(id);
            if (existingCategory == null)
                return false;

            _mapper.Map(categoryDto, existingCategory);
            return await _categoryRepository.UpdateAsync(existingCategory);
        }
    }
}
