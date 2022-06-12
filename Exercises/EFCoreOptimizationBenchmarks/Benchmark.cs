using System.Diagnostics.CodeAnalysis;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using EFCoreOptimizationBenchmarks.Data;
using Microsoft.EntityFrameworkCore;

namespace EFCoreOptimizationBenchmarks;

[MemoryDiagnoser()]
[SuppressMessage("Performance", "CA1822:Mark members as static")]
[InProcess]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[GcServer(true)]
public class Benchmark
{
    private readonly DataContext _dataContext;

    public Benchmark()
    {
        _dataContext = new DataContext();
    }

    [Benchmark]
    public void SelectAllColumns()
    {
        var users = _dataContext.User.ToList();
    }

    [Benchmark]
    public void SelectAllColumnsAsNoTracking()
    {
        var users = _dataContext.User.AsNoTracking().ToList();
    }

    [Benchmark]
    public void SelectSingleColumn()
    {
        var users = _dataContext.User.Select(x => x.FirstName).ToList();
    }
    
    [Benchmark]
    public void SelectSingleColumnAsNoTracking()
    {
        var users = _dataContext.User.AsNoTracking().Select(x => x.FirstName).ToList();
    }

    [Benchmark]
    public void SelectTwoColumns()
    {
        var users = (from u in _dataContext.User
            select new
            {
                u.FirstName,
                u.Age
            }).ToList();
    }
    
    [Benchmark]
    public void SelectTwoColumnsAsNoTracking()
    {
        var users = (from u in _dataContext.User.AsNoTracking()
            select new
            {
                u.FirstName,
                u.Age
            }).ToList();
    }

    [Benchmark]
    public void SelectThreeColumns()
    {
        var users = (from u in _dataContext.User
            select new
            {
                u.FirstName,
                u.Mobile,
                u.Age
            }).ToList();
    }
    
    [Benchmark]
    public void SelectThreeColumnsAsNoTracking()
    {
        var users = (from u in _dataContext.User.AsNoTracking()
            select new
            {
                u.FirstName,
                u.Mobile,
                u.Age
            }).ToList();
    }
    
}