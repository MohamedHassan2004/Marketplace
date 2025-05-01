using Marketplace.Services.DTOs.Auth;
using Microsoft.AspNetCore.Identity;

namespace Marketplace.Services.IService
{
    public interface IAuthService
    {
        Task<LoginResult> Login(LoginDto loginDto);
        Task<RegisterResult> Register(RegisterDto registerDto);
    }
}
