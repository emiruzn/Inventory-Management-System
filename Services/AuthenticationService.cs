using InventoryManagement.Entities;
using InventoryManagement.Repositories;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Services
{
    public class AuthenticationService
    {
        private readonly UserRepository _userRepository;
        private readonly JwtTokenGenerator _jwtTokenGenerator;

        // Constructor to initialize dependencies
        public AuthenticationService(UserRepository userRepository, JwtTokenGenerator jwtTokenGenerator)
        {
            _userRepository = userRepository;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        // Method to authenticate user and generate JWT token
        public async Task<string> AuthenticateAsync(string username, string password)
        {
            var user = await _userRepository.GetUserByUsernameAsync(username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                throw new UnauthorizedAccessException("Invalid username or password.");
            }

            return _jwtTokenGenerator.GenerateToken(user);
        }

        // Method to register a new user with hashed password
        public async Task RegisterAsync(User user, string password)
        {
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
            await _userRepository.AddUserAsync(user);
        }
    }
}
