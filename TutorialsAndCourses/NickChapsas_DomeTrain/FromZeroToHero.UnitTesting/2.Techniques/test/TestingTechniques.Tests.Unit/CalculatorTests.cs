using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Xunit;

namespace TestingTechniques.Tests.Unit;

[ExcludeFromCodeCoverage]
public class CalculatorTests
{
    private readonly Calculator _sut = new();

    [Theory]
    [InlineData(5, 4, 9)]
    [InlineData(-1, 2, 1)]
    [InlineData(-1, -1, -2)]
    [InlineData(0, 0, 0)]
    [InlineData(5, 5, 10)]
    [InlineData(-5, 5, 0)]
    [InlineData(-15, -5, -20)]
    public void Add_ShouldAddTwoNumbers_WhenTwoNumbersAreIntegers(int a, int b, int expected)
    {
        // Act
        var result = _sut.Add(a, b);

        // Assert
        result.Should().Be(expected);
    }
    
    [Theory]
    [InlineData(5, 2, 3)]
    [InlineData(-5, -5, 0)]
    [InlineData(3, 4, -1)]
    [InlineData(0, 0, 0)]
    [InlineData(5, 5, 0)]
    [InlineData(15, 5, 10)]
    [InlineData(-15, -5, -10)]
    [InlineData(5, 10, -5)]
    public void Subtract_ShouldSubtractTwoNumbers_WhenTwoNumbersAreIntegers(int a, int b, int expected)
    {
        // Act
        var result = _sut.Subtract(a, b);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(5, 5, 25)]
    [InlineData(50, 0, 0)]
    [InlineData(-5, -5, 25)]
    [InlineData(-5, 5, -25)]
    public void Multiply_ShouldMultiplyTwoNumbers_WhenTwoNumbersAreIntegers(int a, int b, int expected)
    {
        // Act
        var result = _sut.Multiply(a, b);
        
        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(5, 5, 1)]
    [InlineData(15, 5, 3)]
    public void Divide_ShouldDivideTwoNumbers_WhenTwoNumbersAreIntegers(int a, int b, int expected)
    {
        // Act
        var result = _sut.Divide(a, b);

        // Asset
        result.Should().Be(expected);
    }
    

}