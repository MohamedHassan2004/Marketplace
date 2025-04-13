namespace Marketplace.DAL.Models.Users
{
    public class Customer : ApplicationUser
    {
        public ICollection<ProductReview> ProductReviews { get; set; } = new List<ProductReview>();
        public ICollection<SavedProduct> SavedProducts { get; set; } = new List<SavedProduct>();
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
