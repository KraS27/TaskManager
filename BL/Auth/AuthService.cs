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
        private readonly ILogger<AuthService> _logger;

        public AuthService(IUserRepository userRepository, 
            IConfiguration config, 
            ILogger<AuthService> logger)
        {
            _userRepository = userRepository;
            _config = config;
            _logger = logger;
        }

        public async Task RegisterAsync(RegisterModel registerModel)
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
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            await _userRepository.CreateAsync(user);
            _logger.LogInformation($"User {user.UserName} register at {user.CreatedAt}");
        }

        public async Task<JwtResponseModel> LoginAsync(LoginModel loginModel)
        {
            var user = await _userRepository.GetAsync(loginModel.Login);

            if (user == null)
                throw new NotFoundException($"User with login: {loginModel.Login} not found");

            if (!PasswordHasher.Verify(loginModel.Password, user.PasswordHash))
                throw new InvalidPasswordException("Wrong password");

            int exparationTime = Convert.ToInt32(_config["Jwt:ExpirationTime"]);

            var response = new JwtResponseModel
            {
                AccessToken = JwtProvider.GenerateToken(user, _config),
                ExpirationDate = DateTime.Now.AddHours(exparationTime)
            };
       
            return response;
        }
    }
}
