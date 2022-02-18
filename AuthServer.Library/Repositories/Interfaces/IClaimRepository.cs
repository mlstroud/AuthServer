using System.Security.Claims;
using System.Threading.Tasks;
using AuthServer.Library.DataAccess;

namespace AuthServer.Library.Repositories
{
    public interface IClaimRepository
    {
        Task<DbResponse> AssignClaims(int userId);
        Task<DbResponse<Claim>> GetClaims(int userId);
    }
}
