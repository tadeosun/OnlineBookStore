using EBookStore.Data;
using EBookStore.Models;
using Microsoft.EntityFrameworkCore;

namespace EBookStore.RepositoryImplementation
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<Users?> GetUserByIdAsync(int userId)
        {
            long id = userId;
            return await _context.Users.FindAsync(id);
        }

        public async Task<Users?> GetUserByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<int> AddUserAsync(Users user)
        {
            await _context.Users.AddAsync(user);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateUserAsync(Users user)
        {
            _context.Users.Update(user);
            return await _context.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(Users user)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }

}

