using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.BL.Tasks;
using TaskManager.Entities.DTO.Tasks;

namespace TaskManager.Controllers
{
    [Authorize]
    [ApiController]
    [Route("/tasks")]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;
        private readonly IValidator<CreateTaskModel> _createTaskValidator;
        private readonly IValidator<UpdateTaskModel> _UpdateTaskValidator;
        private readonly ILogger<TaskController> _logger;

        public TaskController(ITaskService taskService, 
            IValidator<CreateTaskModel> createTaskValidator, 
            IValidator<UpdateTaskModel> updateTaskValidator, 
            ILogger<TaskController> logger)
        {
            _taskService = taskService;
            _createTaskValidator = createTaskValidator;
            _UpdateTaskValidator = updateTaskValidator;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var tasks = await _taskService.GetAllAsync();
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while register new user.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "An unexpected error occurred. Please try again later." });
            }
        }
    }
}
