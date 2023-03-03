using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Web;

var builder = WebApplication.CreateBuilder(args);

// Allow requests from API and Blazor client
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            //policy.WithOrigins("https://localhost:7183", "https://localhost:5001");
            policy.WithOrigins("*");
            policy.AllowAnyHeader();
            policy.AllowAnyMethod();
        });
});

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

/*builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ExplicitApiAccessGranted", policy => policy.RequireClaim("API.User"));
});*/

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();