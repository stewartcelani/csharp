using CityInfo.API.Data;
using CityInfo.API.Validation;
using Microsoft.AspNetCore.StaticFiles;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;
config.AddEnvironmentVariables("CityInfoApi_");

builder.Services.AddSingleton<ApplicationDbContext>();

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

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<FileExtensionContentTypeProvider>();

var app = builder.Build();

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

app.Run();