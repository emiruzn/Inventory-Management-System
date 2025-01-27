using InventoryManagement.Entities;

// DTOs designed to handle user operations without exposing the complete User entity.
namespace InventoryManagement.DTOs
{
    public class UserRegisterDto
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
        public required string Email { get; set; }
        public Role Role { get; set; }
    }

    public class UserLoginDto
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
    }

    public class UserUpdateDto
    {
        public required string Username { get; set; }
        public string? Password { get; set; }
        public required string Email { get; set; }
        public Role Role { get; set; }
    }
}
