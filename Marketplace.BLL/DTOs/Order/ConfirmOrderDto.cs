using Marketplace.DAL.Enums;
using System.ComponentModel.DataAnnotations;

namespace Marketplace.Services.DTOs.Order
{
    public class ConfirmOrderDto
    {
        [Required]
        [StringLength(200,MinimumLength=5)]
        public required string ShippingAddress { get; set; }
        [StringLength(200)]
        public string OrderNotes { get; set; }
    }
}
