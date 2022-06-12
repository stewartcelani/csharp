using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Blazor.WasmLogToBrowser.Client;
using Blazor.WasmLogToBrowser.Client.Library;
using Blazor.WasmLogToBrowser.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<DebugService>();
builder.Services.AddScoped<JsConsole>();

await builder.Build().RunAsync();
