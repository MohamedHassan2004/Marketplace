using AutoMapper;
using Marketplace.DAL.Enums;
using Marketplace.DAL.IRepository;
using Marketplace.DAL.Models;
using Marketplace.DAL.Models.Users;
using Marketplace.DAL.Repository;
using Marketplace.Services.DTOs.Product;
using Marketplace.Services.IService;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Data;

namespace Marketplace.Services.Service
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IVendorPermissionRepository _vendorPermissionRepository;

        public ProductService(IProductRepository productRepository, IMapper mapper, IWebHostEnvironment env, IOrderItemRepository orderItemRepository, IVendorPermissionRepository vendorPermissionRepository)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _env = env;
            _orderItemRepository = orderItemRepository;
            _vendorPermissionRepository = vendorPermissionRepository;
        }

        private IEnumerable<TDestination> MapEntitiesWithUserContext<TSource, TDestination>(IEnumerable<TSource> source, string? userId, string? role)
        {
            return _mapper.Map<IEnumerable<TDestination>>(source, opt =>
            {
                opt.Items["userId"] = userId;
                opt.Items["role"] = role;  
            });
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
            var folderPath = Path.Combine(_env.WebRootPath, "images/products");

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

        private async Task CheckAutoApprovalPermissionForProductAsync(string vendorId, Product productEntity)
        {
            try
            {
                var permissions = await _vendorPermissionRepository.GetVendorWithPermissionsDetailsAsync(vendorId);
                if (permissions.Any(p => p.PermissionId == (int)Permissions.AutoApproveProducts))
                {
                    productEntity.ApprovalStatus = ApprovalStatus.Approved;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking vendor permissions: {ex.Message}");
            }
        }

        private static Product CreateProductEntity(ProductCreateDto dto, string vendorId, string fileName)
        {
            return new Product()
            {
                Title = dto.Title,
                VendorId = vendorId,
                Description = dto.Description,
                Price = dto.Price,
                Quantity = dto.Quantity,
                CategoryId = dto.CategoryId,
                ImageUrl = Path.Combine("images/products", fileName).Replace("\\", "/")
            };
        }

        public async Task<bool> AddProductAsync(ProductCreateDto dto, string vendorId)
        {
            if (dto.Image == null || dto.Image.Length == 0)
                throw new ArgumentException("Image is required");

            var fileName = GenerateFileName(dto.Image.FileName);
            var filePath = GetFilePath(fileName);

            await SaveImageToFileSystemAsync(dto.Image, filePath);
            Product productEntity = CreateProductEntity(dto, vendorId, fileName);
            await CheckAutoApprovalPermissionForProductAsync(vendorId, productEntity);

            return await _productRepository.AddAsync(productEntity);
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
                return false;

            DeleteImage(product.ImageUrl);

            return await _productRepository.DeleteByIdAsync(id);
        }

        public async Task<bool> UpdateProductImgAsync(int productId, IFormFile newImg)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null || newImg == null || newImg.Length == 0)
                return false;

            DeleteImage(product.ImageUrl);

            var fileName = GenerateFileName(newImg.FileName);
            var filePath = GetFilePath(fileName);
            await SaveImageToFileSystemAsync(newImg, filePath);

            product.ImageUrl = $"/images/products/{fileName}";
            await _productRepository.UpdateAsync(product);

            return true;
        }

        public async Task<bool> UpdateProductAsync(int Id, ProductUpdateDto product)
        {
            var existingProduct = await _productRepository.GetByIdAsync(Id);
            if (existingProduct == null)
                return false;

            _mapper.Map(product, existingProduct);

            return  await _productRepository.UpdateAsync(existingProduct);
        }

        public async Task<bool> UpdateQuantityAsync(int productId, int quantity)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null) return false;
            if (quantity > 0)
            {
                product.Quantity = quantity;
            }
            else
            {
                var newQuantity = product.Quantity - quantity;
                if (newQuantity >= 0)
                    product.Quantity -= quantity;
                else return false;
            }
                return await _productRepository.UpdateAsync(product);
        }

        public async Task<IEnumerable<ProductHistoryDto>> GetProductHistoryAsync(int productId)
        {
            var orderItems = await _orderItemRepository.GetProductHistoryAsync(productId);
            var historyDto = orderItems.Select(item => new ProductHistoryDto
            {
                CustomerId = item.Order.CustomerId,
                CustomerName = item.Order.Customer.UserName,
                Quantity = item.Quantity,
                OrderedAt = item.Order.ConfirmedAt ?? DateTime.MinValue
            });
            return historyDto;
        }

        /// auto
        public async Task<bool> IncreamentViewsNumber(int productId)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null) return false;
            product.ViewsNumber++;
            return await _productRepository.UpdateAsync(product);
        }


        // customer
        public async Task<IEnumerable<ProductWithSavedDto>> GetAcceptedProductsAsync(string? role, string? userId)
        {
            var acceptedProductsEntities = await _productRepository.GetAcceptedProductsAsync();
            return MapEntitiesWithUserContext<Product, ProductWithSavedDto>(acceptedProductsEntities, userId, role);
        }

        public async Task<IEnumerable<ProductWithSavedDto>> GetProductsByProductNameOrCategoryNameAsync(string? role, string? userId, string Name)
        {
            var acceptedProductsEntities = await _productRepository.GetProductsByProductNameOrCategoryNameAsync(Name);
            return MapEntitiesWithUserContext<Product, ProductWithSavedDto>(acceptedProductsEntities, userId, role);
        }

        public async Task<IEnumerable<ProductWithSavedDto>> GetAcceptedProductsByVendorIdAsync(string? role, string? userId, string vendorId)
        {
            var acceptedProductsEntities = await _productRepository.GetAcceptedProductsByVendorIdAsync(vendorId);
            return MapEntitiesWithUserContext<Product, ProductWithSavedDto>(acceptedProductsEntities, userId, role);
        }

        public async Task<IEnumerable<ProductWithSavedDto>> GetProductsByPriceRangeAsync(string? role, string? userId, decimal minPrice, decimal maxPrice)
        {
            var acceptedProductsEntities = await _productRepository.GetProductsByPriceRangeAsync(minPrice, maxPrice);
            return MapEntitiesWithUserContext<Product, ProductWithSavedDto>(acceptedProductsEntities, userId, role);
        }

        public async Task<IEnumerable<ProductWithSavedDto>> GetProductsByCategoryIdAsync(string? role, string? userId, int categoryId)
        {
            var acceptedProductsEntities = await _productRepository.GetProductsByCategoryIdAsync(categoryId);
            return MapEntitiesWithUserContext<Product, ProductWithSavedDto>(acceptedProductsEntities, userId, role);
        }

        public async Task<ProductWithSavedDto> GetProductByIdAsync(string? role, string? userId, int productId)
        {
            var acceptedProductEntity = await _productRepository.GetProductByIdAsync(productId);
            var dto = _mapper.Map<ProductWithSavedDto>(acceptedProductEntity, opt =>
            { 
                opt.Items["role"] = role;
                opt.Items["userId"] = userId;
            });
            return dto;
        }

        // admin + vendor
        public async Task<IEnumerable<ProductDto>> GetAllProductsByVendorIdAsync(string userId, string vendorId)
        {
            var productsEntities = await _productRepository.GetAllProductsByVendorIdAsync(vendorId);
            var dtos = _mapper.Map<IEnumerable<ProductDto>>(productsEntities, opt=> opt.Items["userId"] = userId);
            return dtos;
        }

        public async Task<IEnumerable<ProductDto>> GetRejectedProductsByVendorIdAsync(string userId, string vendorId)
        {
            var productsEntities = await _productRepository.GetRejectedProductsByVendorIdAsync(vendorId);
            var dtos = _mapper.Map<IEnumerable<ProductDto>>(productsEntities, opt => opt.Items["userId"] = userId);
            return dtos;
        }

        public async Task<IEnumerable<ProductDto>> GetWaitingProductsByVendorIdAsync(string userId, string vendorId)
        {
            var productsEntities = await _productRepository.GetWaitingProductsByVendorIdAsync(vendorId);
            var dtos = _mapper.Map<IEnumerable<ProductDto>>(productsEntities, opt => opt.Items["userId"] = userId);
            return dtos;
        }


        // admin
        public async Task<bool> UpdateProductStatusAsync(int productId , ProductStatusUpdateDto statusDto)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product is null)
                return false;

            product.ApprovalStatus = statusDto.Status;
            product.RejectionReason = statusDto.RejectionReason;
            product.AdminCheckedId = statusDto.AdminCheckedId;

            return await _productRepository.UpdateAsync(product);
        }

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync(string userId)
        {
            var productsEntities = await _productRepository.GetAllProductsAsync();
            var dtos = _mapper.Map<IEnumerable<ProductDto>>(productsEntities, opt => opt.Items["userId"] = userId);
            return dtos;
        }

        public async Task<IEnumerable<ProductDto>> GetRejectedProductsAsync(string userId)
        {
            var productsEntities = await _productRepository.GetRejectedProductsAsync();
            return _mapper.Map<IEnumerable<ProductDto>>(productsEntities, opt => opt.Items["userId"] = userId);
        }

        public async Task<IEnumerable<ProductDto>> GetWaitingProductsAsync(string userId)
        {
            var productsEntities = await _productRepository.GetWaitingProductsAsync();
            return _mapper.Map<IEnumerable<ProductDto>>(productsEntities, opt => opt.Items["userId"] = userId);
        }
    }
}