using Marketplace.DAL.Models.Users;
using Marketplace.Services.DTOs.Auth;
using Marketplace.Services.IService;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Marketplace.Services.Service
{
    public class AuthService : IAuthService
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _config;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AuthService(UserManager<ApplicationUser> userManager, IConfiguration configuration, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _config = configuration;
            _signInManager = signInManager;
        }

        public async Task<LoginResult> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByNameAsync(loginDto.Username);
            if (user == null)
            {
                return LoginResult.Fail("User not found.");
            }

            if (await _userManager.IsLockedOutAsync(user))
            {
                return LoginResult.Fail("User is locked out.");
            }

            bool isAuthenticated = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!isAuthenticated)
            {
                await _userManager.AccessFailedAsync(user);
                return LoginResult.Fail("Invalid password.");
            }

            var roles = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email)
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var token = GenerateToken(claims);

            return LoginResult.Success("", token);
        }


        public async Task<RegisterResult> Register(RegisterDto registerDto)
        {
            var existingUser = await _userManager.FindByNameAsync(registerDto.Username);
            if (existingUser != null)
            {
                return RegisterResult.Fail("Username already exists.");
            }

            ApplicationUser user = null;

            switch (registerDto.Role)
            {
                case "Vendor":
                    user = new Vendor
                    {
                        UserName = registerDto.Username,
                        Email = registerDto.Email,
                    };
                    break;
                case "Customer":
                    user = new Customer
                    {
                        UserName = registerDto.Username,
                        Email = registerDto.Email,
                    };
                    break;
                case "Admin":
                    user = new Admin
                    {
                        UserName = registerDto.Username,
                        Email = registerDto.Email,
                    };
                    break;
            }

            if (user == null)
            {
                return RegisterResult.Fail("Invalid role.");
            }

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                var error = string.Join(", ", result.Errors.Select(e => e.Description));
                return RegisterResult.Fail(error);
            }

            await _userManager.AddToRoleAsync(user, registerDto.Role);
            return RegisterResult.Success("",user.Id);
        }
        


        private string GenerateToken(IList<Claim> claims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            SigningCredentials signingCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256);

            var expire = DateTime.UtcNow.AddDays(2);
            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(claims: claims, expires: expire, signingCredentials: signingCredentials);

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            string token = handler.WriteToken(jwtSecurityToken);

            return token;
        }
    }
}

