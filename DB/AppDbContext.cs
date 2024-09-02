using Microsoft.EntityFrameworkCore;
using TaskManager.Entities.DB;

namespace TaskManager.DB
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<UserModel> Users { get; set; }

        public DbSet<TaskModel> Tasks { get; set; }
    }
}
