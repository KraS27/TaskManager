using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Globalization;
using TaskManager.Entities.DB;
using TaskManager.Entities.DTO.Tasks;
using TaskManager.Entities.Enums;
using TaskManager.Entities.Exceptions;
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

        public async Task<ICollection<TaskModelDTO>?> GetAllAsync(Guid userId, TaskFilters filters)
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

            return await ApplyFilters(filters, tasks).ToListAsync();
        }

        public async Task UpdateAsync()
        {            
            await _context.SaveChangesAsync();
        }

        private IQueryable<TaskModelDTO> ApplyFilters(TaskFilters filters ,IQueryable<TaskModelDTO> query)
        {
            // Apply filters
            if (filters.pageNumber < 1)
                throw new PaginationException("Page must be greater than 0");

            if (filters.pageSize > 20)
                throw new PaginationException("PageSize value should not exceed 20");


            if (filters.status.HasValue)
                query = query.Where(t => t.Status == filters.status.Value);

            if (filters.dueDate.HasValue)
                query = query.Where(t => t.DueDate == filters.dueDate.Value);


            if (filters.priority.HasValue)
                query = query.Where(t => t.Priority == filters.priority.Value);

            // Apply sorting
            switch (filters.sortBy.ToLower())
            {
                case "priority":
                    query = filters.ascending ? query.OrderBy(t => t.Priority) : query.OrderByDescending(t => t.Priority);
                    break;
                default:
                    query = filters.ascending ? query.OrderBy(t => t.DueDate) : query.OrderByDescending(t => t.DueDate);
                    break;
            }

            // Apply pagination        
            int skip = (filters.pageNumber - 1) * filters.pageSize;
            return query.Skip(skip).Take(filters.pageSize);
        }
    }
}
