using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SecureDemo.BlazorWasm;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var test = builder.HostEnvironment.BaseAddress;

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7183/api/") });

builder.Services.AddMsalAuthentication(options =>
{
    options.ProviderOptions.DefaultAccessTokenScopes.Add("api://00a84b97-bc48-493b-8b25-0513bdd5bf1c/API.User");
    builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);
});

await builder.Build().RunAsync();