using System.ComponentModel.DataAnnotations;

namespace Marketplace.Services.DTOs.Auth
{
    public class RegisterDto
    {
        [MaxLength(50)]
        public string Username { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [MaxLength(30)]
        public string Password { get; set; }
        [DataType(DataType.PhoneNumber)]
        [MaxLength(15)]
        public string PhoneNumber { get; set; }
        public string Role { get; set; }
    }
}
