using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace SecureDemo.API.Controllers;

[Authorize]
[Route("api/whoami")]
[RequiredScope("API.User")]
public class WhoamiController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        string owner = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Ok(new { Owner = owner });
    }

}