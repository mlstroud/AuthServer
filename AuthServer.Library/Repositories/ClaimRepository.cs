using System.Data;
using System.Security.Claims;
using System.Threading.Tasks;
using AuthServer.Library.DataAccess;
using Dapper;

namespace AuthServer.Library.Repositories
{
    public class ClaimRepository : IClaimRepository
    {
        private const string AssignClaimsProcedureName = "dbo.Assign_Claims";
        private const string GetClaimsProcedureName = "dbo.Get_Claims";

        private SprocNonQuery _assignClaimsProcedure;
        private SprocQueryMany<Claim> _getClaimsProcedure;

        public async Task<DbResponse> AssignClaims(int userId) =>
            await _assignClaimsProcedure
                .Initialize(AssignClaimsProcedureName, MapAssignClaimsParameters(userId))
                .Execute();

        public async Task<DbResponse<Claim>> GetClaims(int userId) =>
            await _getClaimsProcedure
                .Initialize(GetClaimsProcedureName, MapGetClaimsParameters(userId))
                .Execute();

        private DynamicParameters MapAssignClaimsParameters(int userId)
        {
            var p = new DynamicParameters();

            p.Add(InputParameters.UserId, userId, DbType.Int32);

            return p;
        }

        private DynamicParameters MapGetClaimsParameters(int userId)
        {
            var p = new DynamicParameters();

            p.Add(InputParameters.UserId, userId, DbType.Int32);

            return p;
        }
    }
}
