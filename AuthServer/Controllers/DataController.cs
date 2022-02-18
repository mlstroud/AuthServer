using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using AuthServer.Authorization;

namespace AuthServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes =  JwtBearerDefaults.AuthenticationScheme)]
    public class DataController : ControllerBase
    {
        [HttpGet]
        public IActionResult Data() => Ok();

        [HttpGet]
        [Route("member")]
        [Authorize(Policies.Member)]
        public IActionResult Member() => Ok();

        [HttpGet]
        [Route("admin")]
        [Authorize(Policies.Admin)]
        public IActionResult Admin() => Ok();
    }
}
