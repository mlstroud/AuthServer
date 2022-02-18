using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AuthServer.Library.Services;
using AuthServer.Library.Models.Requests;

namespace AuthServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IClaimService _claimService;
        private readonly ITokenService _tokenService;

        private const int InternalServerErrorCode = (int)HttpStatusCode.InternalServerError;

        public AuthController(IUserService userService, IClaimService claimService, ITokenService tokenService)
        {
            _userService = userService;
            _claimService = claimService;
            _tokenService = tokenService;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (!request.IsValid())
                return BadRequest();

            var registerResult = await _userService.RegisterUser(request);

            if (!registerResult)
                return StatusCode(InternalServerErrorCode);

            var userId = await _userService.GetUserId(request.Username);

            if (userId == 0)
                return StatusCode(InternalServerErrorCode);

            var claimsResult = await _claimService.AssignClaims(userId);

            if (!claimsResult)
                return StatusCode(InternalServerErrorCode);

            return Ok();
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {

            if (!request.IsValid())
                return Unauthorized();

            var user = await _userService.Authenticate(request);

            if (user == null)
                return Unauthorized();

            var claims = await _claimService.GetClaims(user);

            if (claims == null)
                return Unauthorized();

            var token = _tokenService.GetToken(claims);

            return Ok(token);
        }
    }
}
