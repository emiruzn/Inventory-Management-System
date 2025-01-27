using InventoryManagement.DTOs;
using InventoryManagement.Entities;
using InventoryManagement.Repositories;
using BCrypt.Net;

namespace InventoryManagement.Services
{
    public class UserService
    {
        private readonly UserRepository _userRepository;

        // Constructor to initialize UserRepository
        public UserService(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // Method to register a new user
        public async Task RegisterUserAsync(UserRegisterDto userDto)
        {
            // Hash the user's password
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(userDto.Password);
            var user = new User
            {
                Username = userDto.Username,
                PasswordHash = hashedPassword,
                Email = userDto.Email,
                Role = userDto.Role
            };

            // Add the new user to the repository
            await _userRepository.AddUserAsync(user);
        }

        // Method to authenticate a user
        public async Task<User> AuthenticateUserAsync(UserLoginDto loginDto)
        {
            // Retrieve user by username
            var user = await _userRepository.GetUserByUsernameAsync(loginDto.Username);
            // Verify the password
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
                throw new UnauthorizedAccessException("Invalid username or password");

            return user;
        }

        // Method to get a user by ID
        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetUserByIdAsync(id);
        }

        // Method to get all users
        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllUsersAsync();
        }

        // Method to update a user
        public async Task<bool> UpdateUserAsync(int id, UserUpdateDto userDto)
        {
            // Fetch user by ID from the repository
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null) return false;

            // Update user properties
            user.Username = userDto.Username;
            user.Email = userDto.Email;
            user.Role = userDto.Role;

            // Update password if provided
            if (!string.IsNullOrEmpty(userDto.Password))
            {
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password);
            }

            // Save updated user to the repository
            await _userRepository.UpdateUserAsync(user);
            return true;
        }

        // Method to delete a user
        public async Task<bool> DeleteUserAsync(int id)
        {
            // Fetch user by ID from the repository
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null) return false;
            
            // Delete user from the repository
            await _userRepository.DeleteUserAsync(id);
            return true;
        }
    }
}
