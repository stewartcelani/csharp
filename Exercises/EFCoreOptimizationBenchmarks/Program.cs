using EFCoreOptimizationBenchmarks;
using EFCoreOptimizationBenchmarks.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddDbContext<DataContext>();

        var benchmarkSettings = new BenchmarkSettings();
        context.Configuration.GetSection(nameof(BenchmarkSettings)).Bind(benchmarkSettings);
        services.AddSingleton(benchmarkSettings);

        services.AddScoped<EFCoreOptimizationBenchmarks.EFCoreOptimizationBenchmarks>();
    })
    .Build();

await host.Services.RunPendingMigrations();

var entityFrameworkOptimizationBenchmarks = host.Services.GetRequiredService<EFCoreOptimizationBenchmarks.EFCoreOptimizationBenchmarks>();
await entityFrameworkOptimizationBenchmarks.RunAsync(args);