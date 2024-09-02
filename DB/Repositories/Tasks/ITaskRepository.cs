using TaskManager.Entities.DB;
using TaskManager.Entities.DTO.Tasks;
using TaskManager.Entities.Structs;

namespace TaskManager.DB.Repositories.Tasks
{
    public interface ITaskRepository
    {
        public Task<ICollection<TaskModelDTO>?> GetAllAsync(Guid userId, Pagination<TaskModelDTO> pagination);

        public Task<TaskModel?> GetAsync(Guid userId, Guid TaskId);

        public Task<Guid> CreateAsync(TaskModel taskModel);

        public Task UpdateAsync();

        public Task DeleteAsync(TaskModel taskModel);
    }
}
