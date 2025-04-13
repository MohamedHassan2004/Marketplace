using System.ComponentModel.DataAnnotations.Schema;
using Marketplace.DAL.Models.Users;

namespace Marketplace.DAL.Models
{
    public class ProductReview
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string CustomerId { get; set; }
        public string ReviewText { get; set; }
        public int Rating { get; set; }
        public DateTime ReviewDate { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }
        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }
    }
}
