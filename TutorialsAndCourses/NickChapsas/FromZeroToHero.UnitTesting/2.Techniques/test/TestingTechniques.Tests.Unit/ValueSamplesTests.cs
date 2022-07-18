using System;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Xunit;

namespace TestingTechniques.Tests.Unit;

[ExcludeFromCodeCoverage]
public class ValueSamplesTests
{
    private readonly ValueSamples _sut = new();

    [Fact]
    public void StringAssertionExample()
    {
        var fullName = _sut.FullName;

        fullName.Should().Be("Stewart Celani");
        
        fullName.Should().NotBeNullOrEmpty();
        fullName.Should().StartWith("Stewart");
    }

    [Fact]
    public void NumberAssertionExample()
    {
        var age = _sut.Age;

        age.Should().Be(21);
        age.Should().BePositive();
        age.Should().BeGreaterThanOrEqualTo(18);
        age.Should().BeInRange(18, 67);
    }

    [Fact]
    public void DateAssertionExample()
    {
        var dateOfBirth = _sut.DateOfBirth;

        dateOfBirth.Should().Be(new DateOnly(2000, 6, 9));
        dateOfBirth.Should().BeOnOrAfter(new DateOnly(2000, 1, 1));
        dateOfBirth.Should().BeBefore(new DateOnly(2001, 1, 1));
    }

    [Fact]
    public void ObjectAssertionExample()
    {
        var expected = new User
        {
            FullName = "Stewart Celani",
            Age = 21,
            DateOfBirth = new DateOnly(2000, 6, 9)
        };
        
        var expectedAnonymous = new
        {
            FullName = "Stewart Celani",
            Age = 21,
            DateOfBirth = new DateOnly(2000, 6, 9)
        };

        var user = _sut.AppUser;

        // user.Should().Be(expected); // Doesn't work for objects/reference types!
        user.Should().BeEquivalentTo(expected);
        user.Should().BeEquivalentTo(expectedAnonymous);
    }

    [Fact]
    public void EnumerableObjectsAssertionExample()
    {
        var expected = new User
        {
            FullName = "Stewart Celani",
            Age = 21,
            DateOfBirth = new DateOnly(2000, 6, 9)
        };

        var users = _sut.Users.As<User[]>();

        users.Should().ContainEquivalentOf(expected);
        users.Should().HaveCount(3);
        users.Should().Contain(x => x.FullName.StartsWith("Stewart") && x.Age > 5);
    }

    [Fact]
    public void EnumerableNumbersAssertionExample()
    {
        var numbers = _sut.Numbers.As<int[]>();

        numbers.Should().Contain(5);
    }

    [Fact]
    public void ExceptionThrownAssertionExample()
    {
        // Arrange
        var calculator = new Calculator();
        
        // Act
        Action result = () => calculator.Divide(1, 0);

        // Assert
        result.Should().Throw<DivideByZeroException>().WithMessage("Attempted to divide by zero.");
    }

    [Fact]
    public void EventRaisedAssertionExample()
    {
        // Arrange
        var monitorSubject = _sut.Monitor();
        
        // Act
        _sut.RaiseExampleEvent();

        // Assert
        monitorSubject.Should().Raise(nameof(ValueSamples.ExampleEvent));
    }

    [Fact]
    public void TestingInternalMembersExample()
    {
        var number = _sut.InternalSecretNumber;

        number.Should().Be(42);
    }
    
}