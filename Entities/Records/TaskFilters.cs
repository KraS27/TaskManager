using TaskManager.Entities.Enums;

namespace TaskManager.Entities.Structs
{
    public record class TaskFilters
    {       
        public int pageNumber { get; set; } = 1;

        public int pageSize { get; set; } = 5;

        public Status? status { get; set; } = null;

        public DateTime? dueDate { get; set; } = null;

        public Priority? priority { get; set; } = null;

        // default sorting for dueDate
        public string sortBy { get; set; } = "dueDate"; 

        public bool ascending { get; set; } = true;               
    }
}
