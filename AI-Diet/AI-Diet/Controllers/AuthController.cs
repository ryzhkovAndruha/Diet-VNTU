using AI_Diet.Authorization.Services;
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

        public AuthController(IAuthService authService, ITokenService tokenService)
        {
            _authService = authService;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequest loginRequest)
        {
            var loginResponse = await _authService.LoginAsync(loginRequest.Email, loginRequest.Password);

            if (loginResponse == null)
            {
                return BadRequest("Email or Password is invalid");
            }

            Response.Cookies.Append("Refresh-Token", loginResponse.RefreshToken, new CookieOptions() { HttpOnly = true });
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

            return Ok();
        }
    }
}
