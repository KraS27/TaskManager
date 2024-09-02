    using System.Security.Claims;
using TaskManager.DB.Repositories.Tasks;
using TaskManager.DB.Repositories.User;
using TaskManager.Entities.Constants;
using TaskManager.Entities.DB;
using TaskManager.Entities.DTO.Auth;
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
        private readonly Guid userId;

        public TaskService(ITaskRepository taskRepository, 
            IHttpContextAccessor contextAccessor, 
            IUserRepository userRepository)
        {
            _taskRepository = taskRepository;
            _contextAccessor = contextAccessor;
            userId = Guid.Parse(_contextAccessor.HttpContext.User.FindFirstValue(CustomClaims.UserId));
            _userRepository = userRepository;
        }

        public async Task<Guid> CreateAsync(CreateTaskModel createTaskModel)
        {          
            var user = await _userRepository.GetAsync(userId);

            if(user == null) 
                throw new NotFoundException($"User with such id not found");

            var newTask = new TaskModel
            {
                Id = Guid.NewGuid(),
                Title = createTaskModel.Title,
                Description = createTaskModel.Description,
                DueDate = createTaskModel.DueDate,
                Priority = createTaskModel.Priority,
                Status = createTaskModel.Status,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,      
                User = user
            };

            await _taskRepository.CreateAsync(newTask);
            return newTask.Id;
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

        public Task<ICollection<TaskModelDTO>?> GetAllAsync(Pagination<TaskModelDTO> pagination)
        {
            return _taskRepository.GetAllAsync(userId, pagination);
        }

        public async Task UpdateAsync(UpdateTaskModel updateTaskModel)
        {
            var task = await _taskRepository.GetAsync(userId, updateTaskModel.Id);

            if (task == null)
                throw new NotFoundException("Task with such id not found");

            task.Title = updateTaskModel.Title;
            task.Description = updateTaskModel.Description;
            task.Priority = updateTaskModel.Priority;
            task.Status = updateTaskModel.Status;
            task.DueDate = updateTaskModel.DueDate;
            task.UpdatedAt = DateTime.Now;

            await _taskRepository.UpdateAsync();
        }
    }
}
