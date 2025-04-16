using AutoMapper;
using Marketplace.DAL.Enums;
using Marketplace.DAL.IRepository;
using Marketplace.DAL.Models;
using Marketplace.DAL.Models.Users;
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
        public Task<bool> UpdateProductAsync(ProductCreateDto product)
        {
            var productEntity = _mapper.Map<Product>(product);
            return _productRepository.UpdateAsync(productEntity);
        }


        // customer
        public async Task<IEnumerable<AcceptedProductDto>> GetAcceptedProductsAsync(string? customerId)
        {
            var products = await _productRepository.GetAcceptedProductsAsync();
            var dtos = _mapper.Map<IEnumerable<AcceptedProductDto>>(products, opt =>
            {
                opt.Items["customerId"] = customerId;
            });
            return dtos;
        }
        public async Task<IEnumerable<AcceptedProductDto>> GetAcceptedProductsByVendorIdAsync(string? customerId, string vendorId)
        {
            var acceptedProductsEntities = await _productRepository.GetAcceptedProductsByVendorIdAsync(vendorId);
            var dtos = _mapper.Map<IEnumerable<AcceptedProductDto>>(acceptedProductsEntities, opt =>
            {
                opt.Items["customerId"] = customerId;
            });
            return dtos;
        }

        public async Task<IEnumerable<AcceptedProductDto>> GetProductsByPriceRangeAsync(string? customerId, decimal minPrice, decimal maxPrice)
        {
            var acceptedProductsEntities = await _productRepository.GetProductsByPriceRangeAsync(minPrice, maxPrice);
            var dtos = _mapper.Map<IEnumerable<AcceptedProductDto>>(acceptedProductsEntities, opt =>
            {
                opt.Items["customerId"] = customerId;
            });
            return dtos;
        }

        public async Task<IEnumerable<AcceptedProductDto>> GetProductsByCategoryNameAsync(string? customerId, string categoryName)
        {
            var acceptedProductsEntities = await _productRepository.GetProductsByCategoryNameAsync(categoryName);
            var dtos = _mapper.Map<IEnumerable<AcceptedProductDto>>(acceptedProductsEntities, opt =>
            {
                opt.Items["customerId"] = customerId;
            });
            return dtos;
        }

        public async Task<IEnumerable<AcceptedProductDto>> GetProductsByCategoryIdAsync(string? customerId, int categoryId)
        {
            var acceptedProductsEntities = await _productRepository.GetProductsByCategoryIdAsync(categoryId);
            var dtos = _mapper.Map<IEnumerable<AcceptedProductDto>>(acceptedProductsEntities, opt =>
            {
                opt.Items["customerId"] = customerId;
            });
            return dtos;
        }

        public async Task<AcceptedProductDto> GetAcceptedProductByIdAsync(string? customerId, int productId)
        {
            var acceptedProductEntity = await _productRepository.GetProductByIdAsync(productId);
            var dto = _mapper.Map<AcceptedProductDto>(acceptedProductEntity, opt =>
            {
                opt.Items["customerId"] = customerId;
            });
            return dto;
        }

        // admin + vendor 
        //public async Task<IEnumerable<Product>> GetAllProductsByVendorIdAsync(string vendorId)
        //{
        //    var productsEntities = await _productRepository.GetAllProductsByVendorIdAsync(vendorId);
        //    return productsEntities;
        //}

        public async Task<IEnumerable<RejectedProductDto>> GetRejectedProductsByVendorIdAsync(string vendorId)
        {
            var productsEntities = await _productRepository.GetRejectedProductsByVendorIdAsync(vendorId);
            var dtos = _mapper.Map<IEnumerable<RejectedProductDto>>(productsEntities);
            return dtos;
        }

        public async Task<IEnumerable<WaitingProductDto>> GetWaitingProductsByVendorIdAsync(string vendorId)
        {
            var productsEntities = await _productRepository.GetWaitingProductsByVendorIdAsync(vendorId);
            var dtos = _mapper.Map<IEnumerable<WaitingProductDto>>(productsEntities);
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

        //public async Task<IEnumerable<Product>> GetAllProductsAsync()
        //{
        //    var productsEntities = await _productRepository.GetAllProductsAsync();
        //    return productsEntities;
        //}

        public async Task<IEnumerable<RejectedProductDto>> GetRejectedProductsAsync()
        {
            var productsEntities = await _productRepository.GetRejectedProductsAsync();
            return _mapper.Map<IEnumerable<RejectedProductDto>>(productsEntities);
        }

        public async Task<IEnumerable<WaitingProductDto>> GetWaitingProductsAsync()
        {
            var productsEntities = await _productRepository.GetWaitingProductsAsync();
            return _mapper.Map<IEnumerable<WaitingProductDto>>(productsEntities);
        }


        /// auto
        public async Task<bool> IncreamentViewsNumber(int productId)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null) return false;
            product.ViewsNumber++;
            return await _productRepository.UpdateAsync(product);
        }


        ///////////////////////////
        public Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Product>> GetAllProductsByVendorIdAsync(string vendorId)
        {
            throw new NotImplementedException();
        }
    }
}