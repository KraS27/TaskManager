using System.Security.Claims;
using TaskManager.DB.Repositories.Tasks;
using TaskManager.DB.Repositories.User;
using TaskManager.Entities.Constants;
using TaskManager.Entities.DB;
using TaskManager.Entities.DTO.Tasks;
using TaskManager.Entities.Exceptions;
using TaskManager.Entities.Structs;

namespace TaskManager.BL.Tasks
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _contextAccessor;   
        private readonly ILogger<TaskService> _logger;

        public TaskService(ITaskRepository taskRepository,
            IHttpContextAccessor contextAccessor,
            IUserRepository userRepository,
            ILogger<TaskService> logger)
        {
            _taskRepository = taskRepository;
            _contextAccessor = contextAccessor;
            _userRepository = userRepository;
            _logger = logger;
        }

        private Guid GetCurrentUserId()
        {
            var userIdString = _contextAccessor.HttpContext?.User.FindFirstValue(CustomClaims.UserId);
            if (userIdString == null)            
                throw new UnauthorizedAccessException("User is not authenticated.");
            
            return Guid.Parse(userIdString);
        }

        public async Task<Guid> CreateAsync(CreateTaskModel createTaskModel)
        {         
            var userId = GetCurrentUserId();
            var user = await _userRepository.GetAsync(userId);

            if(user == null)
            {
                _logger.LogWarning($"User with id {userId} not found during task creation");
                throw new NotFoundException($"User with such id not found");
            }
                
            var newTask = new TaskModel
            {
                Id = Guid.NewGuid(),
                Title = createTaskModel.Title,
                Description = createTaskModel.Description,
                DueDate = createTaskModel.DueDate,
                Priority = createTaskModel.Priority,
                Status = createTaskModel.Status,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,      
                User = user
            };

            await _taskRepository.CreateAsync(newTask);
            _logger.LogInformation($"Task with id {newTask.Id} created for user {userId}");

            return newTask.Id;
        }

        public async Task DeleteAsync(Guid taskId)
        {
            var userId = GetCurrentUserId();
            var task = await _taskRepository.GetAsync(userId, taskId);

            if (task == null)
            {
                _logger.LogWarning("Attempt to delete non-existing task with id {TaskId} by user {UserId}", taskId, userId);
                throw new NotFoundException($"Task with such id not found");
            }            
            await _taskRepository.DeleteAsync(task);
            _logger.LogInformation("Task with id {TaskId} deleted by user {UserId}", taskId, userId);
        }

        public async Task<TaskModel?> GetAsync(Guid taskId)
        {
            var userId = GetCurrentUserId();
            return await _taskRepository.GetAsync(userId, taskId);
        }

        public Task<ICollection<TaskModelDTO>?> GetAllAsync(TaskFilters filters)
        {
            var userId = GetCurrentUserId();
            return _taskRepository.GetAllAsync(userId, filters);
        }

        public async Task UpdateAsync(UpdateTaskModel updateTaskModel)
        {
            var userId = GetCurrentUserId();
            var task = await _taskRepository.GetAsync(userId, updateTaskModel.Id);

            if (task == null)
            {
                _logger.LogWarning("Attempt to update non-existing task with id {TaskId} by user {UserId}", updateTaskModel.Id, userId);
                throw new NotFoundException("Task with such id not found");
            }            

            task.Title = updateTaskModel.Title;
            task.Description = updateTaskModel.Description;
            task.Priority = updateTaskModel.Priority;
            task.Status = updateTaskModel.Status;
            task.DueDate = updateTaskModel.DueDate;
            task.UpdatedAt = DateTime.UtcNow;

            await _taskRepository.UpdateAsync();
            _logger.LogInformation("Task with id {TaskId} updated by user {UserId}", task.Id, userId);
        }
    }
}
