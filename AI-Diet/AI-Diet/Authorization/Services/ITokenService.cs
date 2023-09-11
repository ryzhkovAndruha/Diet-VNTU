using AI_Diet.Models.RequestModels;
using AI_Diet.Models.ResponseModels;

namespace AI_Diet.Authorization.Services
{
    public interface ITokenService
    {
        string CreateToken(bool isRefreshToken);
        RefreshTokenResponse CreateRefreshTokenResponse(RefreshTokenRequest refreshTokenRequest);
        Task<bool> ValidateRefreshTokenRequestAsync(RefreshTokenRequest token);
    }
}
