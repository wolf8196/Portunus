using System.Dynamic;
using Microsoft.AspNetCore.Mvc;
using Portunus.SSO.Interfaces;

namespace Portunus.Web.Controllers
{
    [Route("api")]
    public class PortunusController : ControllerBase
    {
        private readonly ISSOManager ssoManager;

        public PortunusController(ISSOManager ssoManager)
        {
            this.ssoManager = ssoManager;
        }

        [HttpGet("healthcheck")]
        public IActionResult HealthCheck()
        {
            return Ok();
        }

        [HttpPost("{app}/issue")]
        public IActionResult IssueToken([FromRoute] string app, [FromBody] ExpandoObject payload)
        {
            return Ok(ssoManager.IssueToken(app, payload));
        }

        [HttpGet("{app}/verify/{token}")]
        public IActionResult VerifyToken([FromRoute] string app, [FromRoute] string token)
        {
            if (ssoManager.TryVerifyToken(app, token, out ExpandoObject payload))
            {
                return Ok(payload);
            }
            else
            {
                return Unauthorized();
            }
        }
    }
}