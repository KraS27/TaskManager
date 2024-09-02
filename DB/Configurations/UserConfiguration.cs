using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManager.Entities.DB;

namespace TaskManager.DB.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<UserModel>
    {
        public void Configure(EntityTypeBuilder<UserModel> builder)
        {
            builder.ToTable("users");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.UserName)
                .HasColumnName("userName")
                .HasMaxLength(24)
                .IsRequired();

            builder.Property(u => u.Email)
                .HasColumnName("email")
                .HasMaxLength(40)
                .IsRequired();

            builder.Property(u => u.PasswordHash)
                .HasColumnName("password")                
                .IsRequired();


            builder.HasIndex(u => u.UserName)
                .IsUnique();

            builder.HasIndex(u => u.Email)
                .IsUnique();

            builder.HasMany(u => u.Tasks)
                .WithOne(t => t.User);
        }
    }
}
