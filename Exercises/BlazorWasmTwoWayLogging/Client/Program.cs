global using Blazor.WasmTwoWayLogging.Client.Logging;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Blazor.WasmTwoWayLogging.Client;
using Blazor.WasmTwoWayLogging.Client.Library;
using Blazor.WasmTwoWayLogging.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

Logger.Configure(builder.HostEnvironment.BaseAddress);
Logger.Log.Info("App starting.");

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<DebugService>();
builder.Services.AddScoped<JsConsole>();

await builder.Build().RunAsync();
