using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using TaskManager.DB;
using TaskManager.DB.Repositories.User;
using TaskManager.Entities.DB;
using TaskManager.Entities.DTO.Auth;
using TaskManager.Entities.Exceptions;

namespace TaskManager.BL.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _config;

        public AuthService(IUserRepository userRepository, IConfiguration config)
        {
            _userRepository = userRepository;
            _config = config;
        }

        public async Task Register(RegisterModel registerModel)
        {
            var hashedPassword = PasswordHasher.HashPassword(registerModel.Password);

            var existingUser = await _userRepository.GetAsync(registerModel.UserName, registerModel.Email);

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

            await _userRepository.CreateAsync(user);
        }

        public async Task<string> Login(LoginModel loginModel)
        {
            var user = await _userRepository.GetAsync(loginModel.Login);

            if (user == null)
                throw new NotFoundException($"User with login: {loginModel.Login} not found");

            if (!PasswordHasher.Verify(loginModel.Password, user.PasswordHash))
                throw new InvalidPasswordException("Wrong password");

            var token = JwtProvider.GenerateToken(user, _config);

            return token;
        }
    }
}
