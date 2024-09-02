using TaskManager.Entities.DB;
using TaskManager.Entities.Enums;

namespace TaskManager.Entities.DTO.Tasks
{
    public class TaskModelDTO
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public DateTime? DueDate { get; set; }

        public Status Status { get; set; }

        public Priority Priority { get; set; }        
    }
}
