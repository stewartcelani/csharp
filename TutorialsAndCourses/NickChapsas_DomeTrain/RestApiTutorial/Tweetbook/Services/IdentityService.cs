using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Tweetbook.Data;
using Tweetbook.Domain;
using Tweetbook.Options;

namespace Tweetbook.Services;

public class IdentityService : IIdentityService
{
    private readonly DataContext _dataContext;
    private readonly JwtSettings _jwtSettings;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly TokenValidationParameters _tokenValidationParameters;
    private readonly UserManager<IdentityUser> _userManager;


    public IdentityService(UserManager<IdentityUser> userManager, JwtSettings jwtSettings,
        TokenValidationParameters tokenValidationParameters, DataContext dataContext,
        RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _jwtSettings = jwtSettings;
        _tokenValidationParameters = tokenValidationParameters;
        _dataContext = dataContext;
        _roleManager = roleManager;
    }

    public async Task<AuthenticationResult> RegisterAsync(string email, string password)
    {
        var existingUser = await _userManager.FindByEmailAsync(email);

        if (existingUser is not null)
            return new AuthenticationResult
            {
                Errors = new[] { "User with this email address already exists" }
            };

        var newUser = new IdentityUser
        {
            Email = email,
            UserName = email
        };

        var createdUser = await _userManager.CreateAsync(newUser, password);

        //var createdClaims = await _userManager.AddClaimAsync(newUser, new Claim("tags.view", "true"));

        if (!createdUser.Succeeded)
            return new AuthenticationResult
            {
                Errors = createdUser.Errors.Select(x => x.Description)
            };

        return await GenerateAuthenticationResultForUserAsync(newUser);
    }

    public async Task<AuthenticationResult> LoginAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user is null)
            return new AuthenticationResult
            {
                Errors = new[] { "User does not exist" }
            };

        var userHasValidPassword = await _userManager.CheckPasswordAsync(user, password);

        if (!userHasValidPassword)
            return new AuthenticationResult
            {
                Errors = new[] { "User/password combination is wrong" }
            };

        return await GenerateAuthenticationResultForUserAsync(user);
    }

    public async Task<AuthenticationResult> RefreshTokenAsync(string token, string refreshToken)
    {
        var validatedToken = GetPrincipalFromToken(token);

        if (validatedToken is null)
            return new AuthenticationResult
            {
                Errors = new[] { "Invalid token" }
            };

        var expiryDateUnix = long.Parse(validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);
        var expiryDateTimeUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            .AddSeconds(expiryDateUnix);

        if (expiryDateTimeUtc > DateTime.UtcNow)
            return new AuthenticationResult
            {
                Errors = new[] { "This token hasn't expired yet " }
            };

        var jti = validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
        var userId = validatedToken.Claims.Single(x => x.Type == "id").Value;

        var storedRefreshToken =
            await _dataContext.RefreshTokens.SingleOrDefaultAsync(x => x.Token.ToString() == refreshToken.ToUpper());

        if (storedRefreshToken is null)
            return new AuthenticationResult
            {
                Errors = new[] { "This refresh token does not exist" }
            };

        if (DateTime.UtcNow > storedRefreshToken.ExpiryDate)
            return new AuthenticationResult
            {
                Errors = new[] { "This refresh token has expired" }
            };

        if (storedRefreshToken.Invalidated)
            return new AuthenticationResult
            {
                Errors = new[] { "This refresh token has been invalidated" }
            };

        if (storedRefreshToken.Used)
            return new AuthenticationResult
            {
                Errors = new[] { "This refresh token has been used" }
            };

        if (storedRefreshToken.JwtId != jti || storedRefreshToken.UserId != userId)
            return new AuthenticationResult
            {
                Errors = new[] { "This refresh token does not match this JWT" }
            };

        storedRefreshToken.Used = true;
        _dataContext.RefreshTokens.Update(storedRefreshToken);
        await _dataContext.SaveChangesAsync();

        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
            return new AuthenticationResult
            {
                Errors = new[] { "No user exists matching claim 'id'." }
            };
        return await GenerateAuthenticationResultForUserAsync(user);
    }

    private ClaimsPrincipal? GetPrincipalFromToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        try
        {
            var principal =
                tokenHandler.ValidateToken(token, _tokenValidationParameters, out var validatedToken);
            return !IsJwtWithValidSecurityAlgorithm(validatedToken) ? null : principal;
        }
        catch
        {
            return null;
        }
    }

    private static bool IsJwtWithValidSecurityAlgorithm(SecurityToken validatedToken)
    {
        return validatedToken is JwtSecurityToken jwtSecurityToken &&
               jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                   StringComparison.InvariantCultureIgnoreCase);
    }

    private async Task<AuthenticationResult> GenerateAuthenticationResultForUserAsync(IdentityUser user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Email),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new("id", user.Id)
        };

        var userClaims = await _userManager.GetClaimsAsync(user) ?? new List<Claim>();

        claims.AddRange(userClaims);

        var userRoles = await _userManager.GetRolesAsync(user) ?? new List<string>();

        foreach (var userRole in userRoles)
        {
            claims.Add(new Claim(ClaimTypes.Role, userRole));
            var role = await _roleManager.FindByNameAsync(userRole);
            if (role is null) continue;
            var roleClaims = await _roleManager.GetClaimsAsync(role) ?? new List<Claim>();

            foreach (var roleClaim in roleClaims)
            {
                if (claims.Contains(roleClaim))
                    continue;

                claims.Add(roleClaim);
            }
        }

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.Add(_jwtSettings.TokenLifetime),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        if (token is null)
            return new AuthenticationResult
            {
                Errors = new[] { "Error creating token. " }
            };

        var refreshToken = new RefreshToken
        {
            JwtId = token.Id,
            UserId = user.Id,
            CreationDate = DateTime.UtcNow,
            ExpiryDate = DateTime.UtcNow.AddMonths(6)
        };
        await _dataContext.RefreshTokens.AddAsync(refreshToken);
        await _dataContext.SaveChangesAsync();

        return new AuthenticationResult
        {
            Token = tokenHandler.WriteToken(token),
            RefreshToken = refreshToken.Token.ToString()
        };
    }
}