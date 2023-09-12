using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace AI_Diet.Authorization.Models
{
    public class AuthOptions
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string AccessKey { get; set; }
        public string RefreshKey { get; set; }
        public int AccessTokenLifetime { get; set; }
        public int RefreshTokenLifetime { get; set; }

        public SymmetricSecurityKey GetSymmetricSecurityAccessKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(AccessKey));
        }
        public SymmetricSecurityKey GetSymmetricSecurityRefreshKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(RefreshKey));
        }
    }
}
