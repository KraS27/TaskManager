namespace TaskManager.Entities.DTO.Auth
{
    public class JwtResponseModel
    {
        public string AccessToken { get; set; } = null!;

        public DateTime ExpirationDate { get; set; }

        /// <summary>
        /// for the test task, I decided to not to add a RefreshToken creation logic :\
        /// </summary>
        public string RefreshToken { get; set; } = string.Empty;
    }
}
