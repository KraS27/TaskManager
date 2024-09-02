using TaskManager.Entities.DB;
using TaskManager.Entities.DTO.Tasks;

namespace TaskManager.BL.Tasks
{
    public interface ITaskService
    {
        public Task<ICollection<ForListTaskModel>?> GetAllAsync();

        public Task<TaskModel?> GetAsync(Guid taskId);

        public Task<Guid> CreateAsync(CreateTaskModel createTaskModel);

        public Task UpdateAsync(UpdateTaskModel updateTaskModel);

        public Task DeleteAsync(Guid taskId);
    }
}
