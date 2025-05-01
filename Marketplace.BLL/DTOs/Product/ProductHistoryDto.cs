using System.ComponentModel.DataAnnotations;

namespace Marketplace.Services.DTOs.Product
{
    public class ProductHistoryDto
    {
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int Quantity { get; set; }
        public DateTime OrderedAt { get; set; }
    }
}
