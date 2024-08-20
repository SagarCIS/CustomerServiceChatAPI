using CustomerServiceChatAPI.Auth;
using CustomerServiceChatAPI.Helper;
using CustomerServiceChatAPI.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace CustomerServiceChatAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public LoginController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AdminLoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Find the admin by username
            var admin = await _context.Admins
                .FirstOrDefaultAsync(a => a.Username == model.Username);

            if (admin == null)
            {
                return Unauthorized("Invalid username or password.");
            }

            // Verify the password
            if (!PasswordHelper.VerifyPassword(model.Password, admin.PasswordHash))
            {
                return Unauthorized("Invalid username or password.");
            }

            // Create a cookie
            var options = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7)
            };

            Response.Cookies.Append("AdminInfo", admin.Username, options);

            return Ok("Login successful");
        }
    }
    public class AdminLoginModel
    {
        [Required]
        [StringLength(100)]
        public string Username { get; set; }

        [Required]
        [StringLength(100)]
        public string Password { get; set; }
    }
}
