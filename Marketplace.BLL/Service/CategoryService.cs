using AutoMapper;
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

        private void DeleteImage(string ImgUrl)
        {
            if (!string.IsNullOrEmpty(ImgUrl))
            {
                var imagePath = Path.Combine(_env.WebRootPath, ImgUrl.TrimStart('/'));
                if (File.Exists(imagePath))
                    File.Delete(imagePath);
            }
        }

        private string GenerateFileName(string originalFileName)
        {
            return Guid.NewGuid().ToString() + Path.GetExtension(originalFileName);
        }

        private string GetFilePath(string fileName)
        {
            var folderPath = Path.Combine(_env.WebRootPath, "images/categories");

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            return Path.Combine(folderPath, fileName);
        }

        private async Task SaveImageToFileSystemAsync(IFormFile image, string filePath)
        {
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }
        }

        private Category CreateCategoryEntity(string name, string fileName)
        {
            return new Category
            {
                Name = name,
                ImgUrl = $"/images/categories/{fileName}"
            };
        }

        public async Task<bool> AddCategoryAsync(CategoryCreateDto dto)
        {
            if (dto.Image == null || dto.Image.Length == 0)
                throw new ArgumentException("Image is required");

            var fileName = GenerateFileName(dto.Image.FileName);
            var filePath = GetFilePath(fileName);

            await SaveImageToFileSystemAsync(dto.Image, filePath);

            var category = CreateCategoryEntity(dto.Name, fileName);

            return await _categoryRepository.AddAsync(category);
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
                return false;

            DeleteImage(category.ImgUrl);

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


        private void UpdateCategoryImage(Category category, string newFileName)
        {
            DeleteImage(category.ImgUrl);
            category.ImgUrl = $"/images/categories/{newFileName}";
        }

        public async Task<bool> UpdateCategoryAsync(int id, CategoryCreateDto dto)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
                return false;

            category.Name = dto.Name;

            if (dto.Image?.Length > 0)
            {
                var fileName = GenerateFileName(dto.Image.FileName);
                var filePath = GetFilePath(fileName);

                await SaveImageToFileSystemAsync(dto.Image, filePath);

                UpdateCategoryImage(category, fileName);
            }

            return await _categoryRepository.UpdateAsync(category);
        }  
    }
}
