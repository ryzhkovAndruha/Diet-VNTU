using AI_Diet.Models.RequestModels;
using AI_Diet.Models.ResponseModels;

namespace AI_Diet.Authorization.Services.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponse> LoginAsync(string email, string password);
        Task<RegisterUserResponse> RegisterAsync(RegisterUserRequest registerUserRequestModel);
        Task<DeleteUserResponse> DeleteUserAsync(string userId);
        Task LogoutAsync();
    }
}
