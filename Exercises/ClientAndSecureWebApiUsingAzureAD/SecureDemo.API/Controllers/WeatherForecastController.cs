using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using SecureDemo.Domain;


namespace SecureDemo.API.Controllers;

[ApiController]
[RequiredScope("API.User")]
[Route("api/weatherforecast")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }
    
    [HttpGet]
    [Authorize(Roles="Weather.View")]
    public IEnumerable<WeatherForecast> Get()
    {
        // Get all roles for the current user
        var roles = ((ClaimsIdentity)User.Identity)?.Claims
            .Where(c => c.Type == ClaimTypes.Role)
            .Select(c => c.Value).ToList();

        var httpContext = HttpContext;
        var user = User;
        var owner = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "Unknown";
        var name = User.FindFirst(ClaimTypes.Name)?.Value ?? "Unknown";
        _logger.LogInformation("Requesting weather forecast from: {owner}: {name}", owner, name);
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
    }
}