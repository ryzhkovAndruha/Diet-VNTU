using AI_Diet.Authorization.Models;
using AI_Diet.Authorization.Services;
using AI_Diet.Context;
using AI_Diet.Models.RequestModels;
using AI_Diet.Models.ResponseModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;

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

            Response.Cookies.Append("Refresh-Token", loginResponse.RefreshToken);

            return Ok(loginResponse);
        }

        [HttpPost("refreshToken")]
        public async Task<IActionResult> RefreshAccessTokenAsync(RefreshTokenRequest refreshTokenRequest)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(user => user.Id == refreshTokenRequest.UserId);

            if (user == null)
            {
                return NotFound("User with such id is not found");
            }
            if (user.RefreshToken != refreshTokenRequest.RefreshToken)
            {
                return Unauthorized("Refresh token is wrong");
            }

            return Ok(_authService.CreateRefreshTokenResponse(refreshTokenRequest));
        }

        [HttpPost("logout")]
        public async Task<IActionResult> LogoutAsync()
        {
            await _authService.LogoutAsync();

            return Ok();
        }
    }
}
