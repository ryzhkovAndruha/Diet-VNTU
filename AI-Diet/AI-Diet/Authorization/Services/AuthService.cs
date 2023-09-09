using AI_Diet.Authorization.Models;
using AI_Diet.Models.RequestModels;
using AI_Diet.Models.ResponseModels;
using AI_Diet.Models.UserModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace AI_Diet.Authorization.Services
{
    public class AuthService : IAuthService
    {
        private SignInManager<User> _signInManager;
        private UserManager<User> _userManager;
        private AuthOptions _authOptions;

        public AuthService(SignInManager<User> signInManager, UserManager<User> userManager, AuthOptions authOptions)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _authOptions = authOptions;
        }

        public async Task<LoginResponse> LoginAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var result = await _signInManager.PasswordSignInAsync(user, password, false, false);
            if (!result.Succeeded)
            {
                return default;
            }

            return CreateLoginResponse(user);
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<LoginResponse> RegisterAsync(RegisterUserRequestModel registerUserRequestModel)
        {
            var userToRegister = new User(registerUserRequestModel);
            var result = await _userManager.CreateAsync(userToRegister, registerUserRequestModel.Password);

            if (!result.Succeeded)
            {
                return default;
            }

            return CreateLoginResponse(userToRegister);
        }

        public RefreshTokenResponse CreateRefreshTokenResponse(RefreshTokenRequest refreshTokenRequest)
        {
            return new RefreshTokenResponse()
            {
                UserId = refreshTokenRequest.UserId,
                AccessToken = CreateToken(false)
            };
        }

        private LoginResponse CreateLoginResponse(User user)
        {
            return new LoginResponse
            {
                AccessToken = CreateToken(false),
                RefreshToken = CreateToken(true),
                UserId = user.Id,
                UserName = user.Email,
                FirstName = user.Name,
                LastName = user.SecondName,
            };
        }

        private string CreateToken(bool isRefreshToken)
        {
            var now = DateTime.Now;

            var token = new JwtSecurityToken(
                issuer: _authOptions.Issuer,
                audience: _authOptions.Audience,
                notBefore: now,
                expires: isRefreshToken ? now.AddDays(_authOptions.RefreshTokenLifetime) : now.AddHours(_authOptions.AccessTokenLifetime),
                signingCredentials: new SigningCredentials( isRefreshToken ?
                    _authOptions.GetSymmetricSecurityRefreshKey() : _authOptions.GetSymmetricSecurityAccessKey(),
                    SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
