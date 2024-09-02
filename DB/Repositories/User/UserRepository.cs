using Microsoft.EntityFrameworkCore;
using TaskManager.Entities.DB;
using TaskManager.Entities.DTO.Auth;

namespace TaskManager.DB.Repositories.User
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }

        public async Task CreateAsync(UserModel user)
        {
            await _context.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task<UserModel?> GetAsync(string username, string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(
                x => x.UserName == username ||
                x.Email == email);
        }

        public async Task<UserModel?> GetAsync(string login)
        {
           return await _context.Users
                .FirstOrDefaultAsync(
                x => x.UserName == login ||
                x.Email == login);
        }
    }
}
