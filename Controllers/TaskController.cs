using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.BL.Tasks;
using TaskManager.Entities.DTO.Auth;
using TaskManager.Entities.DTO.Tasks;
using TaskManager.Entities.Exceptions;

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

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            try
            {
                var task = await _taskService.GetAsync(id);

                if(task == null)
                    return NoContent();

                return Ok(task);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while register new user.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "An unexpected error occurred. Please try again later." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTaskModel createTaskModel)
        {
            var validationResult = _createTaskValidator.Validate(createTaskModel);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));

            try
            {
                var newTaskId = await _taskService.CreateAsync(createTaskModel);              

                return Ok(newTaskId);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while register new user.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "An unexpected error occurred. Please try again later." });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {        
            try
            {
                await _taskService.DeleteAsync(id);

                return Ok();
            }
            catch(NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while register new user.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "An unexpected error occurred. Please try again later." });
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody]UpdateTaskModel updateTaskModel)
        {
            var validationResult = _UpdateTaskValidator.Validate(updateTaskModel);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));

            try
            {
                await _taskService.UpdateAsync(updateTaskModel);

                return Ok();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while register new user.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "An unexpected error occurred. Please try again later." });
            }
        }
    }
}
