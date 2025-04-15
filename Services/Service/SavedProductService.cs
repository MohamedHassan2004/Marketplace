using AutoMapper;
using Marketplace.DAL.IRepository;
using Marketplace.DAL.Models;
using Marketplace.Services.DTOs.Product;
using Marketplace.Services.IService;

namespace Marketplace.Services.Service
{
    public class SavedProductService : ISavedProductService
    { 
        private readonly IProductRepository _productRepository;
        private readonly ISavedProductRepository _savedProductRepository;
        private readonly IMapper _mapper;

        public SavedProductService(IProductRepository productRepository, ISavedProductRepository savedProductRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _savedProductRepository = savedProductRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductDto>> GetSavedProductsAsync(string customerId)
        {
            var savedProducts = await _savedProductRepository.GetSavedProductsByCustomerIdAsync(customerId);
            var productDtos = _mapper.Map<IEnumerable<ProductDto>>(savedProducts,opt => opt.Items["customerId"] = customerId);
            return productDtos;
        }


        public async Task<bool> SaveProduct(int productId, string customerId)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null) return false;

            var savedProduct = new SavedProduct
            {
                ProductId = productId,
                CustomerId = customerId
            };

            return await _savedProductRepository.SaveProductAsync(savedProduct);
        }

        public async Task<bool> UnsaveProduct(int productId, string customerId)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null) return false;

            var savedProduct = new SavedProduct
            {
                ProductId = productId,
                CustomerId = customerId
            };

            return await _savedProductRepository.UnsaveProductAsync(savedProduct);
        }
    }

}

