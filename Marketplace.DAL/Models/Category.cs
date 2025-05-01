using System.ComponentModel.DataAnnotations;

namespace Marketplace.DAL.Models
{
    public class Category
    {
        public int Id { get; set; }
        [StringLength(50)]
        public required string Name { get; set; }
        public required string ImgUrl { get; set; }
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
