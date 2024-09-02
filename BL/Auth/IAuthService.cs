using TaskManager.Entities.DTO.Auth;

namespace TaskManager.BL.Auth
{
    public interface IAuthService
    {
        public Task RegisterAsync(RegisterModel registerModel);

        public Task<JwtResponseModel> LoginAsync(LoginModel loginModel);
    }
}
