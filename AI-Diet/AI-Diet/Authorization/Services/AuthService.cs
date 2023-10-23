using AI_Diet.Context;
using AI_Diet.Models.RequestModels;
using AI_Diet.Models.ResponseModels;
using AI_Diet.Models.UserModels;
using Microsoft.AspNetCore.Identity;

namespace AI_Diet.Authorization.Services
{
    public class AuthService : IAuthService
    {
        private SignInManager<User> _signInManager;
        private UserManager<User> _userManager;
        private ITokenService _tokenService;
        private ApplicationContext _dbContext;

        public AuthService(SignInManager<User> signInManager, UserManager<User> userManager, ITokenService tokenService, ApplicationContext dbContext)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _tokenService = tokenService;
            _dbContext = dbContext;
        }

        public async Task<LoginResponse> LoginAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var result = await _signInManager.PasswordSignInAsync(user, password, false, false);

            if (!result.Succeeded)
            {
                return default;
            }

            var loginResponse = CreateLoginResponse(user);
            var loggedInUser = _dbContext.Users.FirstOrDefault(user => user.Id == loginResponse.UserId);

            loggedInUser.RefreshToken = loginResponse.RefreshToken;
            _dbContext.SaveChanges();

            return loginResponse;
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<RegisterUserResponse> RegisterAsync(RegisterUserRequest registerUserRequestModel)
        {
            var userToRegister = new User(registerUserRequestModel);
            var result = await _userManager.CreateAsync(userToRegister, registerUserRequestModel.Password);

            if (!result.Succeeded)
            {
                return new RegisterUserResponseErrors() { RegisterErrors = result.Errors?.Select(x => x.Description).ToList() };
            }

            return CreateRegisterReponse(userToRegister);
        }

        private LoginResponse CreateLoginResponse(User user)
        {
            return new LoginResponse
            {
                AccessToken = _tokenService.CreateToken(false),
                RefreshToken = _tokenService.CreateToken(true),
                UserId = user.Id,
                UserName = user.Email,
                FirstName = user.Name,
                LastName = user.SecondName,
            };
        }

        private RegisterUserResponse CreateRegisterReponse(User user)
        {
            return new RegisterUserResponse
            {
                UserId = user.Id,
                UserName = user.Email,
                FirstName = user.Name,
                LastName = user.SecondName,
            };
        }
    }
}
