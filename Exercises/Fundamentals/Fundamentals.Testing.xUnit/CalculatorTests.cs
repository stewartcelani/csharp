using System.Collections;
using Xunit;

namespace Fundamentals.Testing.xUnit;

public class CalculatorTests
{
    private readonly Calculator _sut;

    public CalculatorTests()
    {
        _sut = new Calculator();
    }

    /*
     * Fact: Single test
     */
    [Fact(Skip = "Example: This test is broken")]
    public void FactExample_AddTwoNumbersShouldEqualTheirEqual()
    {
        _sut.Add(5);
        _sut.Add(8);
        Assert.Equal(13, _sut.Value);
    }


    /*
    * Theory with MemberData
    */
    public static IEnumerable<object[]> AddManyNumbersShouldEqualTheirEqual_Data()
    {
        yield return new object[] { 15, new decimal[] { 10, 5 } };
        yield return new object[] { 15, new decimal[] { 5, 5, 5 } };
        yield return new object[] { -20, new decimal[] { -10, -30, 20 } };
        yield return new object[] { 1, new[] { 0.33m, 0.33m, 0.34m } };
    }

    [Theory]
    [MemberData(nameof(AddManyNumbersShouldEqualTheirEqual_Data))]
    public void AddManyNumbersShouldEqualTheirEqual(
        decimal expected, params decimal[] valuesToAdd)
    {
        foreach (var x in valuesToAdd) _sut.Add(x);

        Assert.Equal(expected, _sut.Value);
    }


    /*
     * InlineData
     */
    [Theory]
    [InlineData(13, 5, 8)]
    [InlineData(0, -3, 3)]
    [InlineData(10.5, 10, 0.5)]
    [InlineData(0, 0, 0)]
    public void AddTwoNumbersShouldEqualTheirEqual(
        decimal expected, decimal x, decimal y)
    {
        _sut.Add(x);
        _sut.Add(y);
        Assert.Equal(expected, _sut.Value);
    }

    [Theory]
    [InlineData(40, 5, 8)]
    [InlineData(9, -3, -3)]
    [InlineData(-9, -3, 3)]
    [InlineData(0, 10, 0)]
    public void MultiplyTwoNumbersShouldEqualTheirEqual(
        decimal expected, decimal x, decimal y)
    {
        _sut.Multiply(x, y);
        Assert.Equal(expected, _sut.Value);
    }

    [Theory]
    [InlineData(40, 5, 8)]
    [InlineData(9, -3, -3)]
    [InlineData(-9, -3, 3)]
    [InlineData(0, 10, 0)]
    public void MultiplyValueByOneNumberShouldEqualTheirEqual(
        decimal expected, decimal startingValue, decimal x)
    {
        _sut.SetValue(startingValue);
        _sut.Multiply(x);
        Assert.Equal(expected, _sut.Value);
    }

    /*
     * ClassData
     * Up to 13:38 in https://www.youtube.com/watch?v=2Wp8en1I9oQ
     */
    public class DivisionTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}