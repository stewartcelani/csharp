using Tweetbook.Extensions;
using Tweetbook.Helpers;
using Tweetbook.Options;

var webApplicationBuilder = WebApplication.CreateBuilder(args);

// Add services to the container.
webApplicationBuilder.AddDbContext();
webApplicationBuilder.AddMvc();
webApplicationBuilder.AddAuth();
webApplicationBuilder.AddCache();
webApplicationBuilder.AddSwagger();
webApplicationBuilder.AddHealthChecks();

var webApplication = webApplicationBuilder.Build();

// Configure the HTTP request pipeline.
if (webApplication.Environment.IsDevelopment())
{
    webApplication.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    webApplication.UseMigrationsEndPoint();
}
else
{
    webApplication.UseHsts();
}

var swaggerOptions = new SwaggerOptions();
webApplicationBuilder.Configuration.GetSection(nameof(SwaggerOptions)).Bind(swaggerOptions);
webApplication.UseSwagger(option => { option.RouteTemplate = swaggerOptions.JsonRoute; });
webApplication.UseSwagger();
webApplication.UseSwaggerUI(
    option => { option.SwaggerEndpoint(swaggerOptions.UiEndpoint, swaggerOptions.Description); });

webApplication.UseHealthChecks("/health", HealthCheckHelper.Options());

webApplication.UseHttpsRedirection();
webApplication.UseStaticFiles();

webApplication.UseRouting();

webApplication.UseAuthentication();
webApplication.UseAuthorization();

webApplication.MapControllers();

// Extension methods
await webApplication.RunPendingMigrations();
await webApplication.EnsureRolesAreSeeded();

webApplication.Run();