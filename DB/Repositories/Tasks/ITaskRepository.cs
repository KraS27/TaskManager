using TaskManager.Entities.DB;
using TaskManager.Entities.DTO.Tasks;

namespace TaskManager.DB.Repositories.Tasks
{
    public interface ITaskRepository
    {
        public Task<ICollection<TaskModel>?> GetAllAsync(Guid userId);

        public Task<TaskModel?> GetAsync(Guid userId, Guid TaskId);

        public Task<Guid> CreateAsync(TaskModel taskModel);

        public Task UpdateAsync();

        public Task DeleteAsync(TaskModel taskModel);
    }
}
