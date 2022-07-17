using System;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace CalculatorLibrary.Tests.Unit;

public class CalculatorTests : IAsyncLifetime
{
    private readonly Calculator _sut = new();
    private readonly ITestOutputHelper _outputHelper;

    public CalculatorTests(ITestOutputHelper outputHelper)
    {
        _outputHelper = outputHelper;
        _outputHelper.WriteLine("Hello from the ctor");
    }

    [Fact]
    public void Add_ShouldAddTwoNumbers_WhenTwoNumbersAreIntegers()
    {
        _outputHelper.WriteLine("Hello from the add unit test");

        // Act
        var result = _sut.Add(5, 4);

        // Assert
        Assert.Equal(9, result);
    }
    
    [Fact]
    public void Subtract_ShouldSubtractTwoNumbers_WhenTwoNumbersAreIntegers()
    {
        _outputHelper.WriteLine("Hello from the subtract unit test");

        // Act
        var result = _sut.Subtract(5, 4);

        // Assert
        Assert.Equal(1, result);
    }

    public async Task InitializeAsync()
    {
        _outputHelper.WriteLine("Hello from InitializeAsync");
        await Task.Delay(1);
    }

    public async Task DisposeAsync()
    {
        _outputHelper.WriteLine("Hello from DisposeAsync");
        await Task.Delay(1);
    }
}