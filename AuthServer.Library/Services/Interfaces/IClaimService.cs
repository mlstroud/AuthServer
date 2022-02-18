using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AuthServer.Library.Models;

namespace AuthServer.Library.Services
{
    public interface IClaimService
    {
        Task<bool> AssignClaims(int userId);
        Task<IEnumerable<Claim>> GetClaims(User user);
    }
}
