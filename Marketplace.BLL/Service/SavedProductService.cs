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

        public SavedProductService(IProductRepository productRepository, ISavedProductRepository savedProductRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _savedProductRepository = savedProductRepository;
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
                    CanBuy = savedProduct.Product.Quantity > 0,
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
                    IsInCart = savedProduct.Product.Orders.Any(o => o.CustomerId == userId && o.Status == OrderStatus.InCart)
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

        public async Task<bool> UnsaveProduct(int SaveId)
        {
            return await _savedProductRepository.UnsaveProductAsync(SaveId);
        }
    }

}

