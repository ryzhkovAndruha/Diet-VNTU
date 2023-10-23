using AI_Diet.Authorization.Services;
using AI_Diet.Models.RequestModels;
using AI_Diet.Models.ResponseModels;
using Microsoft.AspNetCore.Mvc;

namespace AI_Diet.Controllers
{
    [Route("api/user")]
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

            if (registerResponse is RegisterUserResponseErrors registerResponseErrors)
            {
                return BadRequest(registerResponseErrors.RegisterErrors);
            }

            return Ok(registerResponse);
        }
    }
}
