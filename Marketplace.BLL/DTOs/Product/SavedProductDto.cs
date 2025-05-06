namespace Marketplace.Services.DTOs.Product
{
    public class SavedProductDto
    {
        public int SaveId { get; set; }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public DateTime SavedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Quantity { get; set; }
        public int ViewsNumber { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string VendorId { get; set; }
        public string VendorName { get; set; }
        public bool IsSaved { get; set; } = true;
        public bool CanBuy { get; set; } = false;
        public bool IsInCart { get; set; } = false;

    }
}
