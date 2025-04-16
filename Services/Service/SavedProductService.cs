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

        public async Task<IEnumerable<SavedProductDto>> GetSavedProductsAsync(string customerId)
        {
            var savedProducts = await _savedProductRepository.GetSavedProductsByCustomerIdAsync(customerId);
            var productDtos = new List<SavedProductDto>();
            foreach (var savedProduct in savedProducts)
            {
                var dto = new SavedProductDto()
                {
                    SaveId = savedProduct.Id,
                    SavedAt = savedProduct.SavedDate,
                    IsSaved = true,
                    Id = savedProduct.ProductId,
                    Title = savedProduct.Product.Title,
                    Description = savedProduct.Product.Description,
                    ImageUrl = savedProduct.Product.ImageUrl,
                    Price = savedProduct.Product.Price,
                    Quantity = savedProduct.Product.Quantity,
                    CreatedAt = savedProduct.Product.CreatedAt,
                    ViewsNumber = savedProduct.Product.ViewsNumber,
                    CategoryId = savedProduct.Product.CategoryId,
                    CategoryName = savedProduct.Product.Category.Name,
                    VendorId = savedProduct.Product.VendorId,
                    VendorName = savedProduct.Product.Vendor.UserName,
                    AverageRating = savedProduct.Product.Reviews.Any()
                        ? MathF.Round((float)savedProduct.Product.Reviews.Average(r => r.Rating), 2)
                        : 0
                };
                productDtos.Add(dto);
            }
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

        public async Task<bool> UnsaveProduct(int SaveId, string customerId)
        {
            return await _savedProductRepository.UnsaveProductAsync(SaveId);
        }
    }

}

