using TaskManager.Entities.Enums;

namespace TaskManager.Entities.DB
{
    public class TaskModel
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public DateTime? DueDate { get; set; }

        public Status Status { get; set; }

        public Priority Priority { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public UserModel User { get; set; } = null!;
    }
}
