using System.ComponentModel.DataAnnotations;

namespace Marketplace.Services.DTOs.Order
{
    public class CreateOrderItemDto
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        [Range(0, 100,ErrorMessage ="Your Range to Order this Item is ({1},{2})")]
        public int Quantity { get; set; }
        
    }
}