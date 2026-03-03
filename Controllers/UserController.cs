using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using URLShortener.Database;
using URLShortener.Models;
using Microsoft.AspNetCore.Identity;

namespace URLShortener.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly USDbContext _context;
        
        PasswordHasher<User> passwordHasher = new PasswordHasher<User>();

        public UserController(USDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest("All fields are required.");
            }

            if (await _context.isUsernamePresent(request.Username))
            {
                return BadRequest("Username already exists.");
            }

            if (await _context.isEmailPresent(request.Email))
            {
                return BadRequest("Email already exists.");
            }

            User user = new User
            {
                Username = request.Username,
                Email = request.Email,
                Password = ""
            };
            user.Password = passwordHasher.HashPassword(user, request.Password);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return Ok("User registered successfully.");
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest("Username and password are required.");
            }
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
            if (user != null)
            {
                var result = passwordHasher.VerifyHashedPassword(user, user.Password, request.Password);
                if (result == PasswordVerificationResult.Failed)
                {
                    return Unauthorized("Invalid username or password.");
                }
            return Ok("Login successful.");
            }
            return BadRequest("User doesn't exist");
        }
    }
}
