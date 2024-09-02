using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using TaskManager.DB;
using TaskManager.Entities.DB;
using TaskManager.Entities.DTO.Auth;
using TaskManager.Entities.Exceptions;

namespace TaskManager.BL.Auth
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public AuthService(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task Register(RegisterModel registerModel)
        {
            var hashedPassword = PasswordHasher.HashPassword(registerModel.Password);

            var existingUser = await _context.Users
                .FirstOrDefaultAsync(
                x => x.UserName == registerModel.UserName || 
                x.Email == registerModel.Email);

            if (existingUser != null)
            {
                if (existingUser.UserName == registerModel.UserName)
                    throw new AlreadyExistException($"User with such username already exist.");
                if (existingUser.Email == registerModel.Email)
                    throw new AlreadyExistException($"User with such email already exist.");
            }

            var user = new UserModel
            {
                UserName = registerModel.UserName,
                Email = registerModel.Email,
                PasswordHash = hashedPassword,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task<string> Login(LoginModel loginModel)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(
                x => x.UserName == loginModel.Login || 
                x.Email == loginModel.Login);

            if (user == null)
                throw new NotFoundException($"User with login: {loginModel.Login} not found");

            if (!PasswordHasher.Verify(loginModel.Password, user.PasswordHash))
                throw new InvalidPasswordException("Wrong password");

            var token = JwtProvider.GenerateToken(user, _config);

            return token;
        }
    }
}
