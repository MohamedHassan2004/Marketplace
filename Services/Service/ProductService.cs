using AutoMapper;
using Marketplace.DAL.Enums;
using Marketplace.DAL.IRepository;
using Marketplace.DAL.Models;
using Marketplace.DAL.Models.Users;
using Marketplace.DAL.Repository;
using Marketplace.Services.DTOs;
using Marketplace.Services.DTOs.Product;
using Marketplace.Services.IService;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Marketplace.Services.Service
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<bool> AddProductAsync(ProductCreateDto product, string vendorId)
        {
            var productEntity = _mapper.Map<Product>(product);
            productEntity.VendorId = vendorId;
            return await _productRepository.AddAsync(productEntity);
        }

        public async Task<bool> DeleteProductAsync(int productId)
        {
            return await _productRepository.DeleteByIdAsync(productId);
        }
        public async Task<bool> UpdateProductAsync(int Id, ProductCreateDto product)
        {
            var existingProduct = await _productRepository.GetByIdAsync(Id);
            if (existingProduct == null)
                return false;

            _mapper.Map(product, existingProduct);

            return  await _productRepository.UpdateAsync(existingProduct);
        }


        // customer
        public async Task<IEnumerable<ProductDto>> GetAcceptedProductsAsync(string? customerId)
        {
            var products = await _productRepository.GetAcceptedProductsAsync();
            var dtos = _mapper.Map<IEnumerable<ProductDto>>(products, opt =>
            {
                opt.Items["customerId"] = customerId;
            });
            return dtos;
        }
        public async Task<IEnumerable<ProductDto>> GetAcceptedProductsByVendorIdAsync(string? customerId, string vendorId)
        {
            var acceptedProductsEntities = await _productRepository.GetAcceptedProductsByVendorIdAsync(vendorId);
            var dtos = _mapper.Map<IEnumerable<ProductDto>>(acceptedProductsEntities, opt =>
            {
                opt.Items["customerId"] = customerId;
            });
            return dtos;
        }

        public async Task<IEnumerable<ProductDto>> GetProductsByPriceRangeAsync(string? customerId, decimal minPrice, decimal maxPrice)
        {
            var acceptedProductsEntities = await _productRepository.GetProductsByPriceRangeAsync(minPrice, maxPrice);
            var dtos = _mapper.Map<IEnumerable<ProductDto>>(acceptedProductsEntities, opt =>
            {
                opt.Items["customerId"] = customerId;
            });
            return dtos;
        }

        public async Task<IEnumerable<ProductDto>> GetProductsByCategoryNameAsync(string? customerId, string categoryName)
        {
            var acceptedProductsEntities = await _productRepository.GetProductsByCategoryNameAsync(categoryName);
            var dtos = _mapper.Map<IEnumerable<ProductDto>>(acceptedProductsEntities, opt =>
            {
                opt.Items["customerId"] = customerId;
            });
            return dtos;
        }

        public async Task<IEnumerable<ProductDto>> GetProductsByCategoryIdAsync(string? customerId, int categoryId)
        {
            var acceptedProductsEntities = await _productRepository.GetProductsByCategoryIdAsync(categoryId);
            var dtos = _mapper.Map<IEnumerable<ProductDto>>(acceptedProductsEntities, opt =>
            {
                opt.Items["customerId"] = customerId;
            });
            return dtos;
        }

        public async Task<ProductDto> GetAcceptedProductByIdAsync(string? customerId, int productId)
        {
            var acceptedProductEntity = await _productRepository.GetProductByIdAsync(productId);
            var dto = _mapper.Map<ProductDto>(acceptedProductEntity, opt =>
            {
                opt.Items["customerId"] = customerId;
            });
            return dto;
        }

        //admin + vendor
        public async Task<IEnumerable<ProductDto>> GetAllProductsByVendorIdAsync(string vendorId)
        {
            var productsEntities = await _productRepository.GetAllProductsByVendorIdAsync(vendorId);
            var dtos = _mapper.Map<IEnumerable<ProductDto>>(productsEntities);
            return dtos;
        }

        public async Task<IEnumerable<ProductDto>> GetRejectedProductsByVendorIdAsync(string vendorId)
        {
            var productsEntities = await _productRepository.GetRejectedProductsByVendorIdAsync(vendorId);
            var dtos = _mapper.Map<IEnumerable<ProductDto>>(productsEntities);
            return dtos;
        }

        public async Task<IEnumerable<ProductDto>> GetWaitingProductsByVendorIdAsync(string vendorId)
        {
            var productsEntities = await _productRepository.GetWaitingProductsByVendorIdAsync(vendorId);
            var dtos = _mapper.Map<IEnumerable<ProductDto>>(productsEntities);
            return dtos;
        }
        public Task<Product> GetProductByIdAsync(int productId)
        {
            throw new NotImplementedException();
        }

        // admin
        public async Task<bool> UpdateProductStatusAsync(ProductStatusUpdateDto statusDto)
        {
            var product = await _productRepository.GetByIdAsync(statusDto.ProductId);
            if (product is null)
                return false;

            product.ApprovalStatus = statusDto.ApprovalStatus;
            product.RejectionReason = statusDto.ApprovalStatus == ApprovalStatus.Rejected ? statusDto.RejectionReason : null;
            product.AdminCheckedId = statusDto.AdminCheckedId;

            return await _productRepository.UpdateAsync(product);
        }

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            var productsEntities = await _productRepository.GetAllProductsAsync();
            var dtos = _mapper.Map<IEnumerable<ProductDto>>(productsEntities);
            return dtos;
        }

        public async Task<IEnumerable<ProductDto>> GetRejectedProductsAsync()
        {
            var productsEntities = await _productRepository.GetRejectedProductsAsync();
            return _mapper.Map<IEnumerable<ProductDto>>(productsEntities);
        }

        public async Task<IEnumerable<ProductDto>> GetWaitingProductsAsync()
        {
            var productsEntities = await _productRepository.GetWaitingProductsAsync();
            return _mapper.Map<IEnumerable<ProductDto>>(productsEntities);
        }


        /// auto
        public async Task<bool> IncreamentViewsNumber(int productId)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null) return false;
            product.ViewsNumber++;
            return await _productRepository.UpdateAsync(product);
        }



    }
}