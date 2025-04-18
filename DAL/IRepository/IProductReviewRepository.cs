﻿using Marketplace.DAL.Models;

namespace Marketplace.DAL.IRepository
{
    public interface IProductReviewRepository : IRepo<ProductReview>
    {
        Task<IEnumerable<ProductReview>> GetReviewsDetailsAsync();
        Task<IEnumerable<ProductReview>> GetReviewsByProductIdAsync(int productId);
        Task<IEnumerable<ProductReview>> GetReviewsByCustomerIdAsync(string customerId);
    }
}
