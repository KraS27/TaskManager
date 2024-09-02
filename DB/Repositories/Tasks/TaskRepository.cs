using Microsoft.EntityFrameworkCore;
using TaskManager.Entities.DB;
using TaskManager.Entities.DTO.Tasks;

namespace TaskManager.DB.Repositories.Tasks
{
    public class TaskRepository : ITaskRepository
    {
        private readonly AppDbContext _context;

        public TaskRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task Create(TaskModel taskModel)
        {
            await _context.AddAsync(taskModel);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(TaskModel taskModel)
        {
            _context.Remove(taskModel);
            await _context.SaveChangesAsync();
        }

        public async Task<TaskModel?> Get(Guid userId, Guid taskId)
        {
            return await _context.Tasks
                .FirstOrDefaultAsync(t => t.User.Id == userId && t.Id == taskId);
        }

        public async Task<ICollection<TaskModel>?> GetAll(Guid userId)
        {
            return await _context.Tasks
                .Where(t => t.User.Id == userId)
                .ToListAsync();
        }

        public async Task Update(TaskModel taskModel, UpdateTaskModel updateTaskModel)
        {
            taskModel.Title = updateTaskModel.Title;
            taskModel.Description = updateTaskModel.Description;
            taskModel.Priority = updateTaskModel.Priority;
            taskModel.Status = updateTaskModel.Status;
            taskModel.DueDate = updateTaskModel.DueDate;
            taskModel.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
        }
    }
}
