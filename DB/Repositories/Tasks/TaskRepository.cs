using Microsoft.EntityFrameworkCore;
using TaskManager.Entities.DB;
using TaskManager.Entities.DTO.Tasks;
using TaskManager.Entities.Structs;

namespace TaskManager.DB.Repositories.Tasks
{
    public class TaskRepository : ITaskRepository
    {
        private readonly AppDbContext _context;

        public TaskRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> CreateAsync(TaskModel taskModel)
        {
            await _context.AddAsync(taskModel);
            await _context.SaveChangesAsync();

            return taskModel.Id;
        }

        public async Task DeleteAsync(TaskModel taskModel)
        {
            _context.Remove(taskModel);
            await _context.SaveChangesAsync();
        }

        public async Task<TaskModel?> GetAsync(Guid userId, Guid taskId)
        {
            return await _context.Tasks
                .FirstOrDefaultAsync(t => t.User.Id == userId && t.Id == taskId);
        }

        public async Task<ICollection<TaskModelDTO>?> GetAllAsync(Guid userId, Pagination<TaskModelDTO> pagination)
        {
            var tasks = _context.Tasks
                .Where(t => t.User.Id == userId)
                .Select(t => new TaskModelDTO
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    DueDate = t.DueDate,
                    Priority = t.Priority,
                    Status = t.Status,
                })
                .OrderBy(t => t.Id)
                .AsQueryable();

            return await pagination.Apply(tasks).ToListAsync();
        }

        public async Task UpdateAsync()
        {            
            await _context.SaveChangesAsync();
        }
    }
}
