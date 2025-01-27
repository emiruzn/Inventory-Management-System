using InventoryManagement.Entities;
using InventoryManagement.Data;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Repositories
{
    public class UserRepository
    {
        private readonly AppDbContext _context; // Dependency injection for the database context.

        public UserRepository(AppDbContext context)
        {
            _context = context; // Initializes the database context for repository operations.
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            // Retrieves a user by their username. Throws an exception if not found.
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            return user;
        }

        public async Task AddUserAsync(User user)
        {
            // Adds a new user entity to the database.
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync(); // Saves changes to persist the new user.
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            // Retrieves a user by their ID. Throws an exception if not found.
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            return user;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            // Retrieves all users from the database.
            return await _context.Users.ToListAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            // Updates an existing user entity in the database.
            _context.Users.Update(user);
            await _context.SaveChangesAsync(); // Saves changes to apply the update.
        }

        public async Task DeleteUserAsync(int id)
        {
            // Deletes a user by their ID if they exist in the database.
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user); // Marks the user entity for deletion.
                await _context.SaveChangesAsync(); // Saves changes to remove the user.
            }
        }
    }
}
