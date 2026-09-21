using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WorkSphere.AuthService.DTOs;

namespace WorkSphere.AuthService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("login")]
        public IActionResult Login(LoginDto dto)
        {
            if (dto.Username != "admin" || dto.Password != "admin123")
            {
                return Unauthorized(new
                {
                    message = "Invalid username or password"
                });
            }

            var key = _configuration["Jwt:Key"]!;
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, dto.Username),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var securityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(key)
            );

            var credentials = new SigningCredentials(
                securityKey,
                SecurityAlgorithms.HmacSha256
            );

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
            );

            var jwtToken = new JwtSecurityTokenHandler()
                .WriteToken(token);

            return Ok(new
            {
                message = "Login successful",
                token = jwtToken
            });
        }
    }
}