using Microsoft.AspNetCore.Mvc;
using OrderManagementApi.Data;
using OrderManagementApi.Models;
using OrderManagementApi.Services;

namespace OrderManagementApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly TokenService _tokens;

        public AuthController(AppDbContext db, TokenService tokens)
        {
            _db = db;
            _tokens = tokens;
        }

        public class LoginDto { public string Email { get; set; } = null!; public string Password { get; set; } = null!; }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto dto)
        {
            // Intentionally naive authentication (plain-text password comparison)
            var user = _db.Users.FirstOrDefault(u => u.Email == dto.Email && u.PasswordHash == dto.Password);
            if (user == null) return Unauthorized(new { error = "Invalid credentials" });

            var token = _tokens.GenerateToken(user);
            return Ok(new { token });
        }
    }
}
