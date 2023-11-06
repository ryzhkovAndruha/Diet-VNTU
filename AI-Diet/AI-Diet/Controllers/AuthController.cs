using AI_Diet.Authorization.Services.Interfaces;
using AI_Diet.Models.RequestModels;
using Microsoft.AspNetCore.Mvc;

namespace AI_Diet.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IAuthService _authService; 
        private ITokenService _tokenService;
        private IConfiguration _configuration;

        private const string REFRESH_TOKEN_EXPIRES = "RefreshTokenLifetime";

        public AuthController(IAuthService authService, ITokenService tokenService, IConfiguration configuration)
        {
            _authService = authService;
            _tokenService = tokenService;

            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequest loginRequest)
        {
            var loginResponse = await _authService.LoginAsync(loginRequest.Email, loginRequest.Password);

            if (loginResponse == null)
            {
                return BadRequest("Email or Password is invalid");
            }

            var refreshTokenDaysExpirationTime = _configuration.GetValue<int>(REFRESH_TOKEN_EXPIRES);

            Response.Cookies.Append("Refresh-Token", loginResponse.RefreshToken, new CookieOptions() { HttpOnly = true, 
                Expires = DateTimeOffset.Now.AddDays(refreshTokenDaysExpirationTime) });
            return Ok(loginResponse);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshAccessTokenAsync(RefreshTokenRequest refreshTokenRequest)
        {
            try
            {
                await _tokenService.ValidateRefreshTokenRequestAsync(refreshTokenRequest);
            }
            catch (KeyNotFoundException knfe)
            {
                return NotFound(knfe.Message);
            }
            catch (UnauthorizedAccessException uae)
            {
                return Unauthorized(uae.Message);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok(_tokenService.CreateRefreshTokenResponse(refreshTokenRequest));
        }

        [HttpPost("logout")]
        public async Task<IActionResult> LogoutAsync()
        {
            await _authService.LogoutAsync();

            Response.Cookies.Delete("Refresh-Token", new CookieOptions() { HttpOnly = true });
            return Ok();
        }
    }
}
