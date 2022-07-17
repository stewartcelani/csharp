using Microsoft.AspNetCore.Mvc;
using Tweetbook.Contracts.V1;
using Tweetbook.Contracts.V1.Requests;
using Tweetbook.Contracts.V1.Responses;
using Tweetbook.Services;

namespace Tweetbook.Controllers;

[ApiController]
public class IdentityController : ControllerBase
{
    public readonly IIdentityService _identityService;

    public IdentityController(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    [HttpPost(ApiRoutes.Identity.Register)]
    public async Task<IActionResult> Register([FromBody] UserRegistrationRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(new AuthFailedResponse
            {
                Errors = ModelState.Values.SelectMany(x => x.Errors.Select(e => e.ErrorMessage))
            });

        var authResponse = await _identityService.RegisterAsync(request.Email, request.Password);

        if (!authResponse.Success)
            return BadRequest(new AuthFailedResponse
            {
                Errors = authResponse.Errors
            });

        return Ok(new AuthSuccessResponse
        {
            Token = authResponse.Token,
            RefreshToken = authResponse.RefreshToken
        });
    }

    [HttpPost(ApiRoutes.Identity.Login)]
    public async Task<IActionResult> Register([FromBody] UserLoginRequest request)
    {
        var authResponse = await _identityService.LoginAsync(request.Email, request.Password);

        if (!authResponse.Success)
            return BadRequest(new AuthFailedResponse
            {
                Errors = authResponse.Errors
            });

        return Ok(new AuthSuccessResponse
        {
            Token = authResponse.Token,
            RefreshToken = authResponse.RefreshToken
        });
    }

    [HttpPost(ApiRoutes.Identity.Refresh)]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
    {
        var authResponse =
            await _identityService.RefreshTokenAsync(request.Token, request.RefreshToken);

        if (!authResponse.Success)
            return BadRequest(new AuthFailedResponse
            {
                Errors = authResponse.Errors
            });

        return Ok(new AuthSuccessResponse
        {
            Token = authResponse.Token,
            RefreshToken = authResponse.RefreshToken
        });
    }
}