using AI_Diet.Authorization.Services;
using AI_Diet.Context;
using AI_Diet.Models.UserModels;
using Microsoft.AspNetCore.Mvc;

namespace AI_Diet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IAuthService _authService;
        private ApplicationContext _dbContext;

        public UserController(IAuthService authService, ApplicationContext dbContext)
        {
            _authService = authService;
            _dbContext = dbContext;
        }

        [HttpPost("register")]
        public async Task<ActionResult> RegisterUserAsync([FromBody]RegisterUserRequestModel registerUserRequestModel)
        {
            if (registerUserRequestModel.Password != registerUserRequestModel.RepeatPassword)
            {
                return BadRequest("Passwords are not matching");
            }

            var loginResponse = await _authService.RegisterAsync(registerUserRequestModel);

            if (loginResponse == null)
            {
                return BadRequest();
            }

            var loggedInUser = _dbContext.Users.FirstOrDefault(user => user.Id == loginResponse.UserId);
            loggedInUser.RefreshToken = loginResponse.RefreshToken;
            _dbContext.SaveChanges();

            return Ok(loginResponse);
        }
    }
}
