using CustomerServiceChatAPI.Auth;
using CustomerServiceChatAPI.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CustomerServiceChatAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check if the user already exists
            var existingUser = await _context.UserLogin
                .FirstOrDefaultAsync(u => u.Username == user.Username);

            if (existingUser != null)
            {
                return Conflict(new { message = "User already exists." });
            }

            // Save to database
            _context.UserLogin.Add(user);
            await _context.SaveChangesAsync();

            // Create a cookie
            var options = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7)
            };

            Response.Cookies.Append("UserInfo", $"{user.Username}:{user.Category}", options);

            return Ok(user);
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _context.UserLogin.ToListAsync();
            return Ok(users);
        }

        // Delete user function
        [HttpDelete("{username}")]
        public async Task<IActionResult> DeleteUser(string username)
        {
            var user = await _context.UserLogin.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null)
            {
                return NotFound(new { message = "User not found." });
            }

            _context.UserLogin.Remove(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "User deleted successfully." });
        }

        [HttpPut("update-status/{username}")]
        public async Task<IActionResult> UpdateUserStatus(string username, [FromBody] UpdateStatusRequest request)
        {
            var user = await _context.UserLogin.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null)
            {
                return NotFound(new { message = "User not found." });
            }

            user.status = request.Status; // Update the status
            await _context.SaveChangesAsync();

            return Ok(new { message = "User status updated successfully.", user });
        }
        public class UpdateStatusRequest
        {
            public string Status { get; set; }
        }

    }
}
