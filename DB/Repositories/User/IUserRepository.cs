using TaskManager.Entities.DB;

namespace TaskManager.DB.Repositories.User
{
    public interface IUserRepository
    {
        public Task<UserModel?> GetAsync(string username, string email);

        public Task<UserModel?> GetAsync(string login);

        public Task CreateAsync(UserModel user);
    }
}
