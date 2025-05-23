﻿using AutoMapper;
using Marketplace.BLL.Helper;
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

        private static Product CreateProductEntity(ProductCreateDto dto, string vendorId, string uploadImage)
        {
            return new Product()
            {
                Title = dto.Title,
                VendorId = vendorId,
                Description = dto.Description,
                Price = dto.Price,
                Quantity = dto.Quantity,
                CategoryId = dto.CategoryId,
                ImageUrl = uploadImage
            };
        }

        public async Task<bool> AddProductAsync(ProductCreateDto dto, string vendorId)
        {
            var uploadImage = await ImageProcessing.UploadImageAsync(dto.Image, "products", _env);

            Product productEntity = CreateProductEntity(dto, vendorId, uploadImage);
            await CheckAutoApprovalPermissionForProductAsync(vendorId, productEntity);

            return await _productRepository.AddAsync(productEntity);
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
                return false;

            ImageProcessing.DeleteImage(product.ImageUrl,_env);

            return await _productRepository.DeleteByIdAsync(id);
        }

        public async Task<bool> UpdateProductImgAsync(int productId, IFormFile newImg)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null || newImg == null || newImg.Length == 0)
                return false;

            product.ImageUrl = ImageProcessing.UpdateImageAsync(product.ImageUrl, newImg, "products", _env).Result;

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
            product.Quantity = quantity;    
            return await _productRepository.UpdateAsync(product);
        }

        public async Task<IEnumerable<ProductHistoryDto>> GetProductHistoryAsync(int productId)
        {
            var orderItems = await _orderItemRepository.GetProductHistoryAsync(productId);
            var historyDto = orderItems.Select(item => new ProductHistoryDto
            {
                CustomerId = item.Order.CustomerId,
                CustomerName = item.Order.Customer?.UserName ?? "Unknown",
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