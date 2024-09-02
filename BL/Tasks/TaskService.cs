using System.Security.Claims;
using TaskManager.DB.Repositories.Tasks;
using TaskManager.Entities.Constants;
using TaskManager.Entities.DB;
using TaskManager.Entities.DTO.Auth;
using TaskManager.Entities.DTO.Tasks;
using TaskManager.Entities.Exceptions;

namespace TaskManager.BL.Tasks
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly Guid userId;

        public TaskService(ITaskRepository taskRepository, IHttpContextAccessor contextAccessor)
        {
            _taskRepository = taskRepository;
            _contextAccessor = contextAccessor;
            userId = Guid.Parse(_contextAccessor.HttpContext.User.FindFirstValue(CustomClaims.UserId));
        }

        public async Task<Guid> CreateAsync(CreateTaskModel createTaskModel)
        {
            var newTask = new TaskModel
            {
                Title = createTaskModel.Title,
                Description = createTaskModel.Description,
                DueDate = createTaskModel.DueDate,
                Priority = createTaskModel.Priority,
                Status = createTaskModel.Status,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,               
            };

            await _
        }

        public async Task DeleteAsync(Guid taskId)
        {
            var task = await _taskRepository.GetAsync(userId, taskId);

            if (task == null)
                throw new NotFoundException($"Task with such id not found");

            await _taskRepository.DeleteAsync(task);
        }

        public async Task<TaskModel?> GetAsync(Guid taskId)
        {
            return await _taskRepository.GetAsync(userId, taskId);
        }

        public Task<ICollection<TaskModel>?> GetAllAsync()
        {
            return _taskRepository.GetAllAsync(userId);
        }

        public Task UpdateAsync(UpdateTaskModel updateTaskModel)
        {
            throw new NotImplementedException();
        }
    }
}
