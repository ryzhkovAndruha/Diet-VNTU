using AI_Diet.Authorization.Services;
using AI_Diet.Models.RequestModels;
using Microsoft.AspNetCore.Mvc;

namespace AI_Diet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IAuthService _authService;

        public UserController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult> RegisterUserAsync([FromBody]RegisterUserRequest registerUserRequestModel)
        {
            if (registerUserRequestModel.Password != registerUserRequestModel.RepeatPassword)
            {
                return BadRequest("Passwords are not matching");
            }

            var registerResponse = await _authService.RegisterAsync(registerUserRequestModel);

            if (registerResponse == null)
            {
                return BadRequest("Failed to register User");
            }

            return Ok(registerResponse);
        }
    }
}
