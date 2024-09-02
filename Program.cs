
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using TaskManager.BL.Auth;
using TaskManager.DB;
using TaskManager.DB.Repositories.User;
using TaskManager.Entities.DTO.Auth;
using TaskManager.Validators.Auth;

namespace TaskManager
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var MsSqlConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(MsSqlConnectionString);
            });

            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IAuthService, AuthService>();

            builder.Services.AddTransient<IValidator<RegisterModel>, RegisterModelValidator>();
            builder.Services.AddTransient<IValidator<LoginModel>, LoginModelValidator>();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
