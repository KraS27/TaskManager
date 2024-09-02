
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security;
using System.Text;
using System.Text.Json.Serialization;
using TaskManager.BL.Auth;
using TaskManager.BL.Tasks;
using TaskManager.DB;
using TaskManager.DB.Repositories.Tasks;
using TaskManager.DB.Repositories.User;
using TaskManager.Entities.DTO.Auth;
using TaskManager.Entities.DTO.Tasks;
using TaskManager.Validators.Auth;
using TaskManager.Validators.Tasks;

namespace TaskManager
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var MsSqlConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(MsSqlConnectionString);
            });

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    var key = builder.Configuration["Jwt:Key"];
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false,
                        ValidateIssuer = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key!))
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            context.Token = context.Request.Cookies["jwt"];

                            return Task.CompletedTask;
                        }
                    };
                });

            builder.Services.AddAuthorization();

            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<ITaskRepository, TaskRepository>();
            builder.Services.AddScoped<ITaskService, TaskService>();

            builder.Services.AddTransient<IValidator<RegisterModel>, RegisterModelValidator>();
            builder.Services.AddTransient<IValidator<LoginModel>, LoginModelValidator>();
            builder.Services.AddTransient<IValidator<CreateTaskModel>, CreateTaskModelValidator>();
            builder.Services.AddTransient<IValidator<UpdateTaskModel>, UpdateTaskModelValidator>();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
