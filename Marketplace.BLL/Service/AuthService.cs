using Marketplace.DAL.Context;
using Marketplace.DAL.Models;
using Marketplace.DAL.Models.Users;
using Marketplace.Services.DTOs.Auth;
using Marketplace.Services.IService;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Marketplace.BLL.Service
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _config;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly MarketplaceDbContext _context;

        public AuthService(UserManager<ApplicationUser> userManager, IConfiguration config, SignInManager<ApplicationUser> signInManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _config = config;
            _signInManager = signInManager;
            _context = context;
        }

        public async Task<LoginResult> Login(LoginDto loginDto)
        {
            if (string.IsNullOrEmpty(loginDto.Username))
                return LoginResult.Fail("Username is required.");

            var user = await _userManager.FindByNameAsync(loginDto.Username);
            if (user == null)
                return LoginResult.Fail("User not found.");

            if (await _userManager.IsLockedOutAsync(user))
                return LoginResult.Fail("User is locked out.");

            var result = await _signInManager.PasswordSignInAsync(user.UserName!, loginDto.Password, false, false);
            if (!result.Succeeded)
            {
                await _userManager.AccessFailedAsync(user);
                return LoginResult.Fail("Invalid credentials.");
            }

            var claims = await GenerateUserClaims(user);
            var token = GenerateToken(claims);

            var refreshToken = new RefreshToken
            {
                Token = GenerateRefreshToken(),
                UserId = user.Id,
                Created = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddDays(7)
            };

            _context.RefreshTokens.Add(refreshToken);
            await _context.SaveChangesAsync();

            return LoginResult.Success(token, refreshToken.Token);
        }

        public async Task<RegisterResult> Register(RegisterDto registerDto)
        {
            var existingUser = await _userManager.FindByNameAsync(registerDto.Username);
            if (existingUser != null)
                return RegisterResult.Fail("Username already exists.");

            ApplicationUser user = registerDto.Role switch
            {
                "Vendor" => new Vendor { UserName = registerDto.Username, Email = registerDto.Email },
                "Customer" => new Customer { UserName = registerDto.Username, Email = registerDto.Email },
                "Admin" => new Admin { UserName = registerDto.Username, Email = registerDto.Email },
                _ => null!
            };

            if (user == null)
                return RegisterResult.Fail("Invalid role.");

            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return RegisterResult.Fail(errors);
            }

            await _userManager.AddToRoleAsync(user, registerDto.Role);
            return RegisterResult.Success("", user.Id);
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<TokenResult> RefreshTokenAsync(string refreshToken)
        {
            var existingToken = await _context.RefreshTokens.Include(rt => rt.User)
                .FirstOrDefaultAsync(rt => rt.Token == refreshToken);

            if (existingToken == null || !existingToken.IsActive)
                return TokenResult.Fail("Invalid or expired refresh token.");

            // Revoke old token
            existingToken.Revoked = DateTime.UtcNow;

            // Generate new access token & refresh token
            var claims = await GenerateUserClaims(existingToken.User);
            var newJwt = GenerateToken(claims);

            var newRefreshToken = new RefreshToken
            {
                Token = GenerateRefreshToken(),
                UserId = existingToken.UserId,
                Created = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddDays(7)
            };

            _context.RefreshTokens.Add(newRefreshToken);
            await _context.SaveChangesAsync();

            return TokenResult.Success(newJwt, newRefreshToken.Token);
        }

        private async Task<List<Claim>> GenerateUserClaims(ApplicationUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email)
            };
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            return claims;
        }

        private string GenerateToken(IList<Claim> claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiry = DateTime.UtcNow.AddMinutes(15);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: expiry,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            var randomBytes = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }
    }
}
