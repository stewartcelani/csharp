using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Any;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NSubstitute.ReturnsExtensions;
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
        _logger.Received(1)
            .LogError(Arg.Is(sqliteException), Arg.Is("Something went wrong while retrieving all users"));
    }
    

    
    [Fact]
    public async Task GetByIdAsync_ShouldReturnUser_WhenUserExists()
    {
        // Arrange
        var user = new User()
        {
            Id = Guid.NewGuid(),
            FullName = "Stewart Celani"
        };
        _userRepository.GetByIdAsync(user.Id).Returns(user);

        // Act
        var result = await _sut.GetByIdAsync(user.Id);


        // Assert
        result.Should().BeEquivalentTo(user);
        result.Should().NotBeNull();
    }
    
    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenNoUserExists()
    {
        // Arrange
        _userRepository.GetByIdAsync(Arg.Any<Guid>()).ReturnsNull();

        // Act
        var result = await _sut.GetByIdAsync(Guid.NewGuid());

        // Assert
        result.Should().BeNull();
    }
    
    [Fact]
    public async Task GetByIdAsync_ShouldLogMessages_WhenInvoked()
    {
        // Arrange
        var userId = Guid.NewGuid();
        _userRepository.GetByIdAsync(userId).ReturnsNull();

        // Act
        await _sut.GetByIdAsync(userId);

        // Assert
        _logger.Received(1).LogInformation("Retrieving user with id: {0}", userId);
        _logger.Received(1).LogInformation("User with id {0} retrieved in {1}ms", userId, Arg.Any<long>());
    }
    
    [Fact]
    public async Task GetByIdAsync_ShouldLogMessageAndException_WhenExceptionIsThrown()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var sqliteException = new SqliteException("Something went wrong", 500);
        _userRepository.GetByIdAsync(userId).Throws(sqliteException);

        // Act
        var requestAction = async () => await _sut.GetByIdAsync(userId);

        // Assert
        await requestAction.Should().ThrowAsync<SqliteException>().WithMessage("Something went wrong");
        _logger.Received(1)
            .LogError(sqliteException, "Something went wrong while retrieving user with id {0}", userId);
    }

    [Fact]
    public async Task CreateAsync_ShouldCreateUser_WhenUserIsValid()
    {
        // Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            FullName = "Stewart Celani"
        };
        _userRepository.CreateAsync(user).Returns(true);
        
        // Act
        var result = await _sut.CreateAsync(user);
        
        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task CreateAsync_ShouldLogMessages_WhenInvoked()
    {
        // Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            FullName = "Stewart Celani"
        };
        _userRepository.CreateAsync(user).Returns(true);

        // Act
        await _sut.CreateAsync(user);

        // Assert
        _logger.Received(1).LogInformation("Creating user with id {0} and name: {1}", user.Id, user.FullName);
        _logger.Received(1).LogInformation("User with id {0} created in {1}ms", user.Id, Arg.Any<long>());
    }

    [Fact]
    public async Task CreateAsync_ShouldLogMessagesAndException_WhenExceptionIsThrown()
    {
        // Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            FullName = "Stewart Celani"
        };
        var sqliteException = new SqliteException("Something went wrong", 500);
        _userRepository.CreateAsync(user).Throws(sqliteException);

        // Act
        var requestAction = async () => await _sut.CreateAsync(user);

        // Assert
        await requestAction.Should().ThrowAsync<SqliteException>().WithMessage("Something went wrong");
        _logger.Received(1)
            .LogError(sqliteException, "Something went wrong while creating a user");        
    }

    [Fact]
    public async Task DeleteByIdAsync_ShouldDeleteUser_WhenUserExists()
    {
        // Arrange
        _userRepository.DeleteByIdAsync(Arg.Any<Guid>()).Returns(true);

        // Act
        var result = await _sut.DeleteByIdAsync(Guid.NewGuid());
        
        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteByIdAsync_ShouldNotDeleteUser_WhenUserDoesNotExist()
    {
        // Arrange
        _userRepository.DeleteByIdAsync(Arg.Any<Guid>()).Returns(false);

        // Act
        var result = await _sut.DeleteByIdAsync(Guid.NewGuid());

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task DeleteByIdAsync_ShouldLogMessages_WhenDeletingUser()
    {
        // Arrange
        var userId = Guid.NewGuid();
        _userRepository.DeleteByIdAsync(userId).Returns(true);

        // Act
        await _sut.DeleteByIdAsync(userId);
        
        // Assert
        _logger.Received(1).LogInformation("Deleting user with id: {0}", userId);
        _logger.Received(1).LogInformation("User with id {0} deleted in {1}ms", userId, Arg.Any<long>());
    }
    
    [Fact]
    public async Task DeleteByIdAsync_ShouldLogMessagesAndException_WhenExceptionIsThrown()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var sqliteException = new SqliteException("Something went wrong", 500);
        _userRepository.DeleteByIdAsync(userId).Throws(sqliteException);

        // Act
        var requestAction = async () => await _sut.DeleteByIdAsync(userId);

        // Assert
        await requestAction.Should().ThrowAsync<SqliteException>().WithMessage("Something went wrong");
        _logger.Received(1)
            .LogError(sqliteException, "Something went wrong while deleting user with id {0}", userId);        
    }

}