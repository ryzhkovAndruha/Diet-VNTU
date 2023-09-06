using AI_Diet.Models.ResponseModels;
using AI_Diet.Models.UserModels;

namespace AI_Diet.Authorization.Services
{
    public interface IAuthService
    {
        Task<User> LoginAsync(string email, string password);
        LoginResponse CreateLoginResponse(User user);
        Task LogoutAsync();
    }
}
