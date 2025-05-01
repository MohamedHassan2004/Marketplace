using System.ComponentModel.DataAnnotations;

namespace Marketplace.Services.DTOs
{
    public class PatchQuantityDto
    {
        [Range(0, 1000, ErrorMessage = "Your Range to Order this Item is ({1},{2})")]
        public int Quantity { get; set; }
    }
}
