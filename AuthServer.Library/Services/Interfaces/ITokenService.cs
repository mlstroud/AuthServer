using System.Collections.Generic;
using System.Security.Claims;

namespace AuthServer.Library.Services
{
    public interface ITokenService
    {
        string GetToken(IEnumerable<Claim> claims);
    }
}
