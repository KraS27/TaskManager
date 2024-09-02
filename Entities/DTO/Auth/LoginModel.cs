namespace TaskManager.Entities.DTO.Auth
{
    public class LoginModel
    {
        /// <summary>
        /// Can be username or email
        /// </summary>
        public string Login { get; set; } = null!;

        public string Password { get; set; } = null!;
    }
}
