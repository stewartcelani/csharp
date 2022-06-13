using BenchmarkDotNet.Running;
using EFCoreOptimizationBenchmarks.Data;
using Microsoft.Extensions.Logging;

namespace EFCoreOptimizationBenchmarks;

public class App
{
    private readonly BenchmarkSettings _benchmarkSettings;
    private readonly DataContext _dataContext;
    private readonly ILogger<App> _logger;

    public App(ILogger<App> logger,
        DataContext dataContext, BenchmarkSettings benchmarkSettings)
    {
        _logger = logger;
        _dataContext = dataContext;
        _benchmarkSettings = benchmarkSettings;
    }

    public async Task RunAsync(string[] args)
    {
        _logger.LogInformation("App started.");
        
        await _dataContext.RunPendingMigrations();
        await _dataContext.EnsureSeededUsersAndRoles(_benchmarkSettings.RowsToSeed, _logger);

        if (_benchmarkSettings.Enabled) BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
    }
}