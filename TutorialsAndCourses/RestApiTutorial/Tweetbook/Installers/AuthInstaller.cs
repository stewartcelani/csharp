using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Tweetbook.Authorization;
using Tweetbook.Options;
using Tweetbook.Services;

namespace Tweetbook;

public class AuthInstaller : IInstaller
{
    public void InstallServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IIdentityService, IdentityService>();

        var jwtSettings = new JwtSettings();
        configuration.Bind(nameof(JwtSettings), jwtSettings);
        services.AddSingleton(jwtSettings);

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.Secret)),
            ValidateIssuer = false,
            ValidateAudience = false,
            RequireExpirationTime = false,
            ValidateLifetime = true
        };

        services.AddSingleton(tokenValidationParameters);

        services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(option =>
            {
                option.SaveToken = true;
                option.TokenValidationParameters = tokenValidationParameters;
            });

        services.AddAuthorization(option =>
        {
            //option.AddPolicy("TagViewer", builder => builder.RequireClaim("tags.view", new string[]{"true"}));
            option.AddPolicy("MustWorkForMcDonalds",
                policy => { policy.AddRequirements(new WorksForCompanyRequirement("mcdonalds.com")); });
        });
        services.AddSingleton<IAuthorizationHandler, WorksForCompanyHandler>();
    }
}