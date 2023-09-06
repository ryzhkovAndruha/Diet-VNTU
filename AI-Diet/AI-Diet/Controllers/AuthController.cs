﻿using AI_Diet.Authorization.Models;
using AI_Diet.Authorization.Services;
using AI_Diet.Models.RequestModels;
using Microsoft.AspNetCore.Mvc;

namespace AI_Diet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IAuthService _authService;

        public AuthController(IAuthService authService, AuthOptions authOptions)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequest loginRequest)
        {
            var user = await _authService.LoginAsync(loginRequest.Email, loginRequest.Password);

            if (user == null)
            {
                return BadRequest();
            }

            var loginResponse = _authService.CreateLoginResponse(user);

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