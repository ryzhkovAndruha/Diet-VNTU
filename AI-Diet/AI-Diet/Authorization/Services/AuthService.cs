using AI_Diet.Authorization.Models;
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

        public async Task<User> LoginAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var result = await _signInManager.PasswordSignInAsync(user, password, false, false);
            if (!result.Succeeded)
            {
                return default;
            }

            return user;
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public LoginResponse CreateLoginResponse(User user)
        {
            return new LoginResponse
            {
                AccessToken = CreateToken(),
                UserId = user.Id,
                UserName = user.Email,
                FirstName = user.Name,
                LastName = user.Name,
            };
        }

        private string CreateToken()
        {
            var now = DateTime.Now;

            var token = new JwtSecurityToken(
                issuer: _authOptions.Issuer,
                audience: _authOptions.Audience,
                notBefore: now,
                expires: now.AddHours(_authOptions.Lifetime),
                signingCredentials: new SigningCredentials(
                    _authOptions.GetSymmetricSecurityKey(),
                    SecurityAlgorithms.HmacSha256)
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
