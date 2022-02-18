using System.Threading.Tasks;
using System.Security.Claims;
using System.Collections.Generic;
using AuthServer.Library.Models;
using AuthServer.Library.DataAccess;
using AuthServer.Library.Repositories;
using Serilog;

namespace AuthServer.Library.Services
{
    public class ClaimService : IClaimService
    {
        private readonly IClaimRepository _claimRepository;

        public ClaimService(IClaimRepository claimRepository)
        {
            _claimRepository = claimRepository;
        }

        public async Task<bool> AssignClaims(int userId)
        {
            var result = await _claimRepository.AssignClaims(userId);

            if (result.Status != DbResponseStatus.Success)
            {
                Log.Error($"Unable to assign claims for user id [{userId}]. Status: [{result.Status.ToString()}].");
                return false;
            }

            return true;
        }

        public async Task<IEnumerable<Claim>> GetClaims(User user)
        {
            var claims = await _claimRepository.GetClaims(user.Id);

            if (claims.Status != DbResponseStatus.Success)
            {
                Log.Error($"Unable to get claims for user id: [{user.Id}]. Status: [{claims.Status.ToString()}].");
                return null;
            }

            var finalClaims = (List<Claim>)claims.Values;

            finalClaims.Add(new Claim("username", user.Username));

            return finalClaims;
        }
    }
}
