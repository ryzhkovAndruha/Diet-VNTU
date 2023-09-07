using AI_Diet.Authorization.Models;
using AI_Diet.Authorization.Services;
using AI_Diet.Context;
using AI_Diet.Models.RequestModels;
using Microsoft.AspNetCore.Mvc;

namespace AI_Diet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IAuthService _authService;
        private ApplicationContext _dbContext;

        public AuthController(IAuthService authService, ApplicationContext dbContext)
        {
            _authService = authService;
            _dbContext = dbContext;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequest loginRequest)
        {
            var loginResponse = await _authService.LoginAsync(loginRequest.Email, loginRequest.Password);

            if (loginResponse == null)
            {
                return BadRequest();
            }

            var loggedInUser = _dbContext.Users.FirstOrDefault(user => user.Id == loginResponse.UserId);
            loggedInUser.RefreshToken = loginResponse.RefreshToken;
            _dbContext.SaveChanges();

            return Ok(loginResponse);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> LogoutAsync()
        {
            await _authService.LogoutAsync();

            return Ok();
        }

    }
}
