using BenchmarkDotNet.Running;
using EFCoreOptimizationBenchmarks.Data;
using Microsoft.Extensions.Logging;

namespace EFCoreOptimizationBenchmarks;

public class EFCoreOptimizationBenchmarks
{
    private readonly BenchmarkSettings _benchmarkSettings;
    private readonly DataContext _dataContext;
    private readonly ILogger<EFCoreOptimizationBenchmarks> _logger;

    public EFCoreOptimizationBenchmarks(ILogger<EFCoreOptimizationBenchmarks> logger,
        DataContext dataContext, BenchmarkSettings benchmarkSettings)
    {
        _logger = logger;
        _dataContext = dataContext;
        _benchmarkSettings = benchmarkSettings;
    }

    public async Task RunAsync(string[] args)
    {
        await _dataContext.EnsureSeededUsersAndRoles(_benchmarkSettings.RowsToSeed, _logger);

        if (_benchmarkSettings.Enabled) BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
    }
}