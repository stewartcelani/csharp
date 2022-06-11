using Microsoft.AspNetCore.Mvc;
using Tweetbook.Filters;

namespace Tweetbook.Controllers;

[ApiKeyAuth]
[ApiController]
public class SecretController : ControllerBase
{
    [HttpGet("secret")]
    public IActionResult GetSecret()
    {
        return Ok("I have no secrets");
    }
}