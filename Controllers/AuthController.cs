using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using TaskManager.BL.Auth;
using TaskManager.Entities.DTO.Auth;
using TaskManager.Entities.Exceptions;

namespace TaskManager.Controllers
{
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IValidator<RegisterModel> _registerValidator;
        private readonly IValidator<LoginModel> _loginValidator;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, 
            IValidator<RegisterModel> registerValidator, 
            IValidator<LoginModel> loginValidator, 
            ILogger<AuthController> logger)
        {
            _authService = authService;
            _registerValidator = registerValidator;
            _loginValidator = loginValidator;
            _logger = logger;
        }

        [HttpPost("/users/register")]
        public async Task<IActionResult> Register(RegisterModel registerModel)
        {
            var validationResult = _registerValidator.Validate(registerModel);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));

            try
            {
                await _authService.RegisterAsync(registerModel);
                return Ok();
            }
            catch (AlreadyExistException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while register new user.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "An unexpected error occurred. Please try again later." });
            }
        }

        [HttpPost("/users/login")]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            var validationResult = _loginValidator.Validate(loginModel);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));

            try
            {
                var response = await _authService.LoginAsync(loginModel);
                HttpContext context = HttpContext;

                //for ease of use with swagger
                context.Response.Cookies.Append("jwt", response.AccessToken);

                return Ok(response);
            }
            catch (NotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidPasswordException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while register new user.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "An unexpected error occurred. Please try again later." });
            }
        }
    }
}
