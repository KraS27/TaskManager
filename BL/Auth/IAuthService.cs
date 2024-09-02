using TaskManager.Entities.DTO.Auth;

namespace TaskManager.BL.Auth
{
    public interface IAuthService
    {
        public Task Register(RegisterModel registerModel);

        public Task<string> Login(LoginModel loginModel);
    }
}
