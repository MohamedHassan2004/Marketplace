using AutoMapper;
using Marketplace.BLL.Helper;
using Marketplace.DAL.IRepository;
using Marketplace.DAL.Models;
using Marketplace.DAL.Repository;
using Marketplace.Services.DTOs.Category;
using Marketplace.Services.IService;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Marketplace.Services.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper, IWebHostEnvironment env)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _env = env;
        }


        public async Task<bool> AddCategoryAsync(CategoryCreateDto dto)
        {
            var uploadImage = await ImageProcessing.UploadImageAsync(dto.Image, "categories", _env);

            var category = new Category
            {
                Name = dto.Name,
                ImgUrl = uploadImage
            };

            return await _categoryRepository.AddAsync(category);
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
                return false;

            ImageProcessing.DeleteImage(category.ImgUrl,_env);

            return await _categoryRepository.DeleteByIdAsync(category.Id);
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


        private void UpdateCategoryImage(Category category, CategoryCreateDto dto)
        {
            category.ImgUrl = ImageProcessing.UpdateImageAsync(category.ImgUrl, dto.Image, "categories", _env).Result;
        }

        public async Task<bool> UpdateCategoryAsync(int id, CategoryCreateDto dto)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
                return false;

            category.Name = dto.Name;

            if (dto.Image?.Length > 0)
            {
                UpdateCategoryImage(category, dto);
            }

            return await _categoryRepository.UpdateAsync(category);
        }  
    }
}
