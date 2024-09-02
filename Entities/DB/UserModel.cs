namespace TaskManager.Entities.DB
{
    public class UserModel
    {
        Guid Id { get; set; }

        public string UserName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string PasswordHash { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set;} = DateTime.UtcNow;

        public ICollection<TaskModel>? Tasks { get; set; }
    }
}
