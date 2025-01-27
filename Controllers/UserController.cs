using Microsoft.AspNetCore.Mvc;
using InventoryManagement.DTOs;
using InventoryManagement.Services;
using InventoryManagement.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace InventoryManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly JwtTokenGenerator _jwtTokenGenerator;

        public UsersController(UserService userService, JwtTokenGenerator jwtTokenGenerator)
        {
            _userService = userService;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        // Endpoint for registering a user. Requires "AdminPolicy" authorization. POST /api/users/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto userDto)
        {
            var currentUser = HttpContext.User;
            var isAdmin = currentUser.IsInRole("Admin");
            
            // Handle the case where a non-admin user tries to assign a role other than Viewer.
            if (!isAdmin && userDto.Role != Role.Viewer)
            {
                return Forbid("Only Admin users can assign roles other than Viewer.");
            }

            await _userService.RegisterUserAsync(userDto);
            return Ok(new { message = "User registered successfully" });
        }

        // Endpoint for logging in a user. POST /api/users/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto loginDto)
        {
            var user = await _userService.AuthenticateUserAsync(loginDto);
            if (user == null)
            {
                return Unauthorized();
            }

            // Generate a JWT token for the user.
            var token = _jwtTokenGenerator.GenerateToken(user);
            return Ok(new { token });
        }

        // Endpoint for getting a user by ID. Requires "AdminPolicy" authorization. GET /api/users/{id}
        [Authorize(Policy = "AdminPolicy")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        // Endpoint for getting all users. Requires "AdminPolicy" authorization. GET /api/users
        [Authorize(Policy = "AdminPolicy")]
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        // Endpoint for updating a user. Requires "AdminPolicy" authorization. PUT /api/users/{id}
        [Authorize(Policy = "AdminPolicy")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserUpdateDto userDto)
        {
            var result = await _userService.UpdateUserAsync(id, userDto);
            if (!result) return NotFound();
            return NoContent();
        }

        // Endpoint for deleting a user. Requires "AdminPolicy" authorization. DELETE /api/users/{id}
        [Authorize(Policy = "AdminPolicy")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _userService.DeleteUserAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
