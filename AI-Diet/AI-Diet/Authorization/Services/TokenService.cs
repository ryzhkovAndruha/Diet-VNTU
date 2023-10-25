using AI_Diet.Authorization.Models;
using AI_Diet.Authorization.Services.Interfaces;
using AI_Diet.Context;
using AI_Diet.Models.RequestModels;
using AI_Diet.Models.ResponseModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace AI_Diet.Authorization.Services
{
    public class TokenService : ITokenService
    {
        private AuthOptions _authOptions;
        private ApplicationContext _dbContext;
        public TokenService(AuthOptions authOptions, ApplicationContext dbContext)
        {
            _authOptions = authOptions;
            _dbContext = dbContext;
        }

        public string CreateToken(bool isRefreshToken)
        {
            var now = DateTime.Now;

            var token = new JwtSecurityToken(
                issuer: _authOptions.Issuer,
                audience: _authOptions.Audience,
                notBefore: now,
                expires: isRefreshToken ? now.AddDays(_authOptions.RefreshTokenLifetime) : now.AddHours(_authOptions.AccessTokenLifetime),
                signingCredentials: new SigningCredentials(isRefreshToken ?
                    _authOptions.GetSymmetricSecurityRefreshKey() : _authOptions.GetSymmetricSecurityAccessKey(),
                    SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public RefreshTokenResponse CreateRefreshTokenResponse(RefreshTokenRequest refreshTokenRequest)
        {
            return new RefreshTokenResponse()
            {
                UserId = refreshTokenRequest.UserId,
                AccessToken = CreateToken(false)
            };
        }

        public async Task<bool> ValidateRefreshTokenRequestAsync(RefreshTokenRequest refreshTokenRequest)
        {
            JwtSecurityToken jwtSecurityToken;
            try
            {
                jwtSecurityToken = new JwtSecurityToken(refreshTokenRequest.RefreshToken);
            }
            catch (Exception)
            {
                throw;
            }

            var user = await _dbContext.Users.FirstOrDefaultAsync(user => user.Id == refreshTokenRequest.UserId);

            if (user == null)
            {
                throw new KeyNotFoundException("User with such id is not found");
            }
            if (user.RefreshToken != refreshTokenRequest.RefreshToken)
            {
                throw new UnauthorizedAccessException("Refresh token is wrong");
            }
            if (jwtSecurityToken.ValidTo < DateTime.Now)
            {
                throw new UnauthorizedAccessException("Refresh token is expired");
            }

            return true;
        }


    }
}
