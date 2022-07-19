using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Users.Api.Logging;
using Users.Api.Models;
using Users.Api.Repositories;
using Users.Api.Services;
using Xunit;

namespace Users.Api.Tests.Unit;

[ExcludeFromCodeCoverage]
public class UserServiceTests
{
    private readonly UserService _sut;
    private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
    private readonly ILoggerAdapter<UserService> _logger = Substitute.For<ILoggerAdapter<UserService>>();

    public UserServiceTests()
    {
        _sut = new UserService(_userRepository, _logger);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnEmptyList_WhenNoUsersExist()
    {
        // Arrange
        _userRepository.GetAllAsync().Returns(Enumerable.Empty<User>());

        // Act
        var result = await _sut.GetAllAsync();

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnUsers_WhenSomeUsersExist()
    {
        // Arrange
        var expectedUsers = new[]
        {
            new User
            {
                Id = Guid.NewGuid(),
                FullName = "Stewart Celani"
            },
            new User
            {
                Id = Guid.NewGuid(),
                FullName = "George Kotsas"
            }
        };
        _userRepository.GetAllAsync().Returns(expectedUsers);

        // Act
        var result = await _sut.GetAllAsync();

        // Assert
        result.Should().BeEquivalentTo(expectedUsers);
        result.Count().Should().Be(expectedUsers.Length);
    }

    [Fact]
    public async Task GetAllAsync_ShouldLogMessages_WhenInvoked()
    {
        // Arrange
        _userRepository.GetAllAsync().Returns(Enumerable.Empty<User>());

        // Act
        await _sut.GetAllAsync();

        // Assert
        _logger.Received(1).LogInformation(Arg.Is<string?>(x => x!.StartsWith("Retrieving")));
        _logger.Received(1).LogInformation(Arg.Is("All users retrieved in {0}ms"), Arg.Any<long>());
    }

    [Fact]
    public async Task GetAllAsync_ShouldLogMessageAndException_WhenExceptionIsThrown()
    {
        // Arrange
        var sqliteException = new SqliteException("Something went wrong", 500);
        _userRepository.GetAllAsync().Throws(sqliteException);

        // Act
        var requestAction = async () => await _sut.GetAllAsync();

        // Assert
        await requestAction.Should().ThrowAsync<SqliteException>().WithMessage("Something went wrong");
        _logger.Received(1).LogError(Arg.Is(sqliteException), Arg.Is("Something went wrong while retrieving all users"));
    }
}