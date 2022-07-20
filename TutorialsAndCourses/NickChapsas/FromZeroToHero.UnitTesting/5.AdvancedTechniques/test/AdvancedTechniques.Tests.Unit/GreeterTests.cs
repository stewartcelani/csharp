using System;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace AdvancedTechniques.Tests.Unit;

public class GreeterTests
{
    private readonly Greeter _sut;
    private readonly IDateTimeProvider _dateTimeProvider = Substitute.For<IDateTimeProvider>();

    public GreeterTests()
    {
        _sut = new Greeter(_dateTimeProvider);
    }
    
    [Fact]
    public void GenerateGreetMessage_ShouldSayGoodEvening_WhenItsEvening()
    {
        // Arrange
        _dateTimeProvider.DateTimeNow.Returns(new DateTime(2020, 1, 1, 20, 0, 0));

        // Act
        var result = _sut.GenerateGreetMessage();

        // Assert
        result.Should().Be("Good evening");
    }

    [Fact]
    public void GenerateGreetMessage_ShouldSayGoodMorning_WhenItsMorning()
    {
        // Arrange
        _dateTimeProvider.DateTimeNow.Returns(new DateTime(2020, 1, 1, 10, 0, 0));

        // Act
        var result = _sut.GenerateGreetMessage();

        // Assert
        result.Should().Be("Good morning");
    }

    [Fact]
    public void GenerateGreetMessage_ShouldSayGoodAfternoon_WhenItsAfternoon()
    {
        // Arrange
        _dateTimeProvider.DateTimeNow.Returns(new DateTime(2020, 1, 1, 15, 0, 0));

        // Act
        var result = _sut.GenerateGreetMessage();

        // Assert
        result.Should().Be("Good afternoon");
    }
}