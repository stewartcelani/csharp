using System.IO;
using CityInfo.API.Data;
using CityInfo.API.Domain.Settings;
using CityInfo.API.Domain.Settings.Helpers;
using CityInfo.API.Logging;
using CityInfo.API.Middleware;
using CityInfo.API.Repositories;
using CityInfo.API.Services;
using CityInfo.API.Validators;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    ContentRootPath = Directory.GetCurrentDirectory()
});

var config = builder.Configuration;
config.AddEnvironmentVariables("CityInfoApi_");

builder.Services.AddControllers(options => { options.ReturnHttpNotAcceptable = true; })
    .AddNewtonsoftJson()
    .AddXmlDataContractSerializerFormatters()
    .AddFluentValidation(x =>
    {
        //x.RegisterValidatorsFromAssemblyContaining<Program>();
        x.DisableDataAnnotationsValidation = true;
    });

var mailSettings = SettingsBinder.BindAndValidate<MailSettings, MailSettingsValidator>(config);
builder.Services.AddSingleton(mailSettings);

var databaseSettings = SettingsBinder.BindAndValidate<DatabaseSettings, DatabaseSettingsValidator>(config);
builder.Services.AddSingleton(databaseSettings);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<FileExtensionContentTypeProvider>();
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(config)
    .CreateLogger();
builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.ClearProviders();
    loggingBuilder.AddSerilog(Log.Logger, true);
});
builder.Services.AddTransient(typeof(ILoggerAdapter<>), typeof(LoggerAdapter<>));

builder.Services.AddDbContext<ApplicationDbContext>();

builder.Services.AddTransient<ICityRepository, CityRepository>();
builder.Services.AddTransient<IPointOfInterestRepository, PointOfInterestRepository>();
builder.Services.AddTransient<ICityService, CityService>();
builder.Services.AddTransient<IPointOfInterestService, PointOfInterestService>();

builder.Services.AddScoped<IUriService>(provider =>
{
    var accessor = provider.GetRequiredService<IHttpContextAccessor>();
    var request = accessor!.HttpContext!.Request;
    var absoluteUri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent(), "/");
    return new UriService(absoluteUri);
});

#if DEBUG
builder.Services.AddTransient<IMailService, LocalMailService>();
#else
builder.Services.AddTransient<IMailService, CloudMailService>();
#endif


var app = builder.Build();


app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<ApiExceptionMiddleware>();
app.UseMiddleware<ValidationExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

await app.RunPendingMigrationsAsync();
await app.SeedDataAsync();

app.Run();