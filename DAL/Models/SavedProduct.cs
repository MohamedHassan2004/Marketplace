using System.ComponentModel.DataAnnotations.Schema;
using Marketplace.DAL.Models.Users;

namespace Marketplace.DAL.Models
{
    public class SavedProduct
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string CustomerId { get; set; }
        public DateTime SavedDate { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }
    }
}
