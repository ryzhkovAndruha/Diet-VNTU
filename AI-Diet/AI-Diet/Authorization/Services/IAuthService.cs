using AI_Diet.Models.RequestModels;
using AI_Diet.Models.ResponseModels;
using AI_Diet.Models.UserModels;

namespace AI_Diet.Authorization.Services
{
    public interface IAuthService
    {
        Task<LoginResponse> LoginAsync(string email, string password);
        Task<LoginResponse> RegisterAsync(RegisterUserRequestModel registerUserRequestModel);
        RefreshTokenResponse CreateRefreshTokenResponse(RefreshTokenRequest refreshTokenRequest);
        Task LogoutAsync();
    }
}
