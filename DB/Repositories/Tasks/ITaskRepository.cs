using TaskManager.Entities.DB;
using TaskManager.Entities.DTO.Tasks;

namespace TaskManager.DB.Repositories.Tasks
{
    public interface ITaskRepository
    {
        public Task<ICollection<TaskModel>?> GetAll(Guid userId);

        public Task<TaskModel?> Get(Guid userId, Guid TaskId);

        public Task Create(TaskModel createTaskModel);

        public Task Update(TaskModel taskModel, UpdateTaskModel updateTaskModel);

        public Task Delete(TaskModel taskModel);
    }
}
