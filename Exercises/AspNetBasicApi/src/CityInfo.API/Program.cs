using CityInfo.API.Data;
using CityInfo.API.Domain.Settings;
using CityInfo.API.Domain.Settings.Helpers;
using CityInfo.API.Logging;
using CityInfo.API.Middleware;
using CityInfo.API.Repositories;
using CityInfo.API.Services;
using CityInfo.API.Validators;
using Microsoft.AspNetCore.StaticFiles;
using FluentValidation.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;
config.AddEnvironmentVariables("CityInfoApi_");

builder.Services.AddControllers(options =>
{
    options.ReturnHttpNotAcceptable = true;
})
    .AddNewtonsoftJson()
    .AddXmlDataContractSerializerFormatters()
    .AddFluentValidation(x =>
    {
        x.RegisterValidatorsFromAssemblyContaining<Program>();
        x.DisableDataAnnotationsValidation = true;
    });

var mailSettings = SettingsBinder.BindAndValidate<MailSettings, MailSettingsValidator>(config);
builder.Services.AddSingleton(mailSettings);

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

builder.Services.AddSingleton<ApplicationDbContext>();
builder.Services.AddSingleton<ICityRepository, CityRepository>();
builder.Services.AddSingleton<IPointOfInterestRepository, PointOfInterestRepository>();
builder.Services.AddSingleton<ICityService, CityService>();

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
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

var dbContext = app.Services.GetRequiredService<ApplicationDbContext>();
await dbContext.SeedDataAsync();

app.Run();