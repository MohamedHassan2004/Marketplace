using Marketplace.Services.DTOs.Auth;
using Marketplace.Services.IService;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Marketplace.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var result = await _authService.Login(loginDto);
            if (result.IsSuccess)
            {
                return Ok(new
                {
                    token = result.Token,
                    refreshToken = result.RefreshToken
                });
            }
            return BadRequest(new { message = result.ErrorMessage });
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            var result = await _authService.Register(registerDto);
            if (result.IsSuccess)
            {
                return Ok(new { message = "User registered successfully!", userId = result.UserId });
            }
            return BadRequest(new { message = result.ErrorMessage });
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] string refreshTokenDto)
        {
            if (string.IsNullOrEmpty(refreshTokenDto))
            {
                return BadRequest(new { message = "Refresh token is required" });
            }

            var result = await _authService.RefreshTokenAsync(refreshTokenDto);
            if (result.IsSuccess)
            {
                return Ok(new
                {
                    token = result.Token,
                    refreshToken = result.RefreshToken
                });
            }

            return BadRequest(new { message = result.ErrorMessage });
        }
    }
}
