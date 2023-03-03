using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace BlazorWasmAzureAD;

public class GraphApiAuthorizationMessageHandler : AuthorizationMessageHandler
{
    public GraphApiAuthorizationMessageHandler(IAccessTokenProvider provider,
        NavigationManager navigationManager)
        : base(provider, navigationManager)
    {
        ConfigureHandler(
            authorizedUrls: new[] { "https://graph.microsoft.com" },
            scopes: new[] { "https://graph.microsoft.com/User.Read", "https://graph.microsoft.com/Mail.Read" });
    }
}