using System.ComponentModel.DataAnnotations.Schema;
using Marketplace.DAL.Models.Users;

namespace Marketplace.DAL.Models
{
    public class SavedProduct
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public required string CustomerId { get; set; }
        public DateTime SavedDate { get; set; } = DateTime.Now;
        public virtual Product Product { get; set; }
        public virtual Customer Customer { get; set; }
    }
}
