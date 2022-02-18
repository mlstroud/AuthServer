using System.Text;
using System.Security.Claims;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using AuthServer.Library.Config;

namespace AuthServer.Library.Services
{
    public class TokenService : ITokenService
    {
        private readonly IAuthConfig _authConfig;
        private readonly JwtSecurityTokenHandler _tokenHandler;

        public TokenService(IAuthConfig config)
        {
            _authConfig = config;
            _tokenHandler = new JwtSecurityTokenHandler();
        }

        public string GetToken(IEnumerable<Claim> claims)
        {
            var token = _tokenHandler.CreateToken(GetTokenDescriptor(claims));

            return _tokenHandler.WriteToken(token);
        }

        private SecurityTokenDescriptor GetTokenDescriptor(IEnumerable<Claim> claims) =>
            new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = _authConfig.GetTokenExpiration(),
                SigningCredentials = GetSigningCredentials()
            };

        private SigningCredentials GetSigningCredentials() =>
            new SigningCredentials(GetSecurityKey(), SecurityAlgorithms.HmacSha256Signature);

        private SymmetricSecurityKey GetSecurityKey() =>
            new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_authConfig.Secret));
    }
}
