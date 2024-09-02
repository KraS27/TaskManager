using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManager.Entities.DB;

namespace TaskManager.DB.Configurations
{
    public class TaskConfiguration : IEntityTypeConfiguration<TaskModel>
    {
        public void Configure(EntityTypeBuilder<TaskModel> builder)
        {
            builder.ToTable("tasks");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.Title)
                .HasColumnName("title")
                .HasMaxLength(68)
                .IsRequired();

            builder.Property(u => u.Description)
                .HasColumnName("description")
                .HasMaxLength(512);           

            builder.HasOne(t => t.User)
                .WithMany(u => u.Tasks);
        }
    }
}
