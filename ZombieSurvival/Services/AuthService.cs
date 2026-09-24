using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ZombieSurvival.DTOs;

namespace ZombieSurvival.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        public AuthService(UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }
        public async Task<AuthResponse?> RegisterAsync(RegisterRequest request)
        {
            var user = new IdentityUser { UserName = request.Email, Email = request.Email };
            var results = await _userManager.CreateAsync(user, request.Password);

            if (!results.Succeeded)
            {
                return null;
            }

            return CreateTokenResponse(user);

        }
        public async Task<AuthResponse?> LoginAsync(LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user is null)
            {
                return null;
            }

            var valid = await _userManager.CheckPasswordAsync(user, request.Password);


            if (!valid)
            {
                return null;
            }

            return CreateTokenResponse(user);
        }

        private AuthResponse CreateTokenResponse(IdentityUser user)
        {
            //Read the same JWT settings from Program.cs
            var key = _configuration["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key is missing.");
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];
            var expiresMinutes = int.Parse(_configuration["Jwt:ExpiresInMinutes"] ?? "60");

            //Set expiration 
            var expiresAt = DateTime.UtcNow.AddMinutes(expiresMinutes);

            //Adding claims to know who the user is
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id), // stable user id for later Player.UserId
                new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
            };

            //Create the signing key
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            //Build the token itslef
            var token = new JwtSecurityToken
            (
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: expiresAt,
                signingCredentials: credentials
            );

            //Serialize to a string
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            //Return your DTO
            return new AuthResponse
            {
                Token = tokenString,
                ExpiresAt = expiresAt,
                Email = user.Email ?? string.Empty
            };
        }
    }
}
