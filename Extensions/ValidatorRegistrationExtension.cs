using FluentValidation;
using TaskManager.Entities.DTO.Auth;
using TaskManager.Entities.DTO.Tasks;
using TaskManager.Validators.Auth;
using TaskManager.Validators.Tasks;

namespace TaskManager.Extensions
{
    public static class ValidatorRegistrationExtension
    {
        public static void RegisterValidators(this IServiceCollection services)
        {
            services.AddTransient<IValidator<RegisterModel>, RegisterModelValidator>();
            services.AddTransient<IValidator<LoginModel>, LoginModelValidator>();

            services.AddTransient<IValidator<CreateTaskModel>, CreateTaskModelValidator>();
            services.AddTransient<IValidator<UpdateTaskModel>, UpdateTaskModelValidator>();
        }
    }
}
