using AutoMapper;
using Marketplace.DAL.Enums;
using Marketplace.DAL.IRepository;
using Marketplace.DAL.Models;
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

        public async Task<bool> AddProductAsync(ProductCreateDto product)
        {
            var productEntity = _mapper.Map<Product>(product);
            return await _productRepository.AddAsync(productEntity);
        }

        public async Task<bool> DeleteProductAsync(int productId)
        {
            return await _productRepository.DeleteByIdAsync(productId);
        }
        public Task<bool> UpdateProductAsync(Product product)
        {
            var productEntity = _mapper.Map<Product>(product);
            return _productRepository.UpdateAsync(productEntity);
        }


        // customer
        public async Task<IEnumerable<ProductDto>> GetAcceptedProductsAsync(string CustomerId)
        {
            var products = await _productRepository.GetAcceptedProductsAsync();
            var dtos = _mapper.Map<IEnumerable<ProductDto>>(products, opt =>
            {
                opt.Items["cusomerId"] = CustomerId;
            });
            return dtos;
        }
        public async Task<IEnumerable<ProductDto>> GetAcceptedProductsByVendorIdAsync(string vendorId)
        {
            var acceptedProductsEntities = await _productRepository.GetAcceptedProductsByVendorIdAsync(vendorId);
            var productDtos = _mapper.Map<IEnumerable<ProductDto>>(acceptedProductsEntities);
            return productDtos;
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