using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Users.Api.Contracts;
using Users.Api.Controllers;
using Users.Api.Mappers;
using Users.Api.Models;
using Users.Api.Repositories;
using Users.Api.Services;
using Xunit;

namespace Users.Api.Tests.Unit;

[ExcludeFromCodeCoverage]
public class UserControllerTests
{
    private readonly UserController _sut;
    private readonly IUserService _userService = Substitute.For<IUserService>();

    public UserControllerTests()
    {
        _sut = new UserController(_userService);
    }

    [Fact]
    public async Task GetById_ShouldReturnOkAndObject_WhenUserExists()
    {
        // Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            FullName = "Stewart Celani"
        };
        _userService.GetByIdAsync(user.Id).Returns(user);
        var userResponse = user.ToUserResponse();

        // Act
        var result = (OkObjectResult)await _sut.GetById(user.Id);

        // Assert
        result.StatusCode.Should().Be(200);
        result.Value.Should().BeEquivalentTo(userResponse);
    }
    
    [Fact]
    public async Task GetById_ShouldReturnNotFound_WhenUserDoesntExist()
    {
        // Arrange
        _userService.GetByIdAsync(Arg.Any<Guid>()).ReturnsNull();

        // Act
        var result = (NotFoundResult)await _sut.GetById(Guid.NewGuid());

        // Assert
        result.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task GetAll_ShouldReturnEmptyList_WhenNoUsersExist()
    {
        // Arrange
        _userService.GetAllAsync().Returns(Enumerable.Empty<User>());

        // Act
        var result = (OkObjectResult)await _sut.GetAll();
        
        // Assert
        result.StatusCode.Should().Be(200);
        result.Value.As<IEnumerable<UserResponse>>().Should().BeEmpty();
    }
    
    [Fact]
    public async Task GetAll_ShouldReturnUsersResponse_WhenUsersExist()
    {
        // Arrange
        var users = new List<User>()
        {
            new ()
            {
                Id = Guid.NewGuid(),
                FullName = "Stewart Celani"
            },
            new ()
            {
                Id = Guid.NewGuid(),
                FullName = "John Smith"
            }
        };
        _userService.GetAllAsync().Returns(users);
        var usersResponse = users.Select(x => x.ToUserResponse()).ToList();

        // Act
        var result = (OkObjectResult)await _sut.GetAll();
        
        // Assert
        result.StatusCode.Should().Be(200);
        result.Value.As<IEnumerable<UserResponse>>().Should().BeEquivalentTo(usersResponse);
        result.Value.As<IEnumerable<UserResponse>>().Should().HaveCount(users.Count);
    }

    [Fact]
    public async Task Create_ShouldCreateAndReturnUser_WhenCreateUserRequestIsValid()
    {
        // Arrange
        var createUserRequest = new CreateUserRequest
        {
            FullName = "Stewart Celani"
        };
        var user = new User
        {
            Id = Guid.NewGuid(),
            FullName = createUserRequest.FullName
        };
        // Arg.Do is setting the user passed to _userService.CreateAsync to the user object above so we can assert on the Id
        _userService.CreateAsync(Arg.Do<User>(x => user = x)).Returns(true);
        
        // Act
        var result = (CreatedAtActionResult)await _sut.Create(createUserRequest);
        
        // Assert
        var expectedUserResponse = user.ToUserResponse();
        result.StatusCode.Should().Be(201);
        result.Value.As<UserResponse>().Should().BeEquivalentTo(expectedUserResponse);
        result.RouteValues!["id"].Should().BeEquivalentTo(user.Id);
        // A naive way to exclude the Id from being validated
        // result.Value.As<UserResponse>().Should()
        //    .BeEquivalentTo(expectedUserResponse, options => options.Excluding(x => x.Id));
    }

    [Fact]
    public async Task Create_ShouldReturnBadRequest_WhenCreateUserRequestIsInvalid()
    {
        // Arrange
        _userService.CreateAsync(Arg.Any<User>()).Returns(false);

        // Act
        var result = (BadRequestResult)await _sut.Create(new CreateUserRequest());

        // Assert
        result.StatusCode.Should().Be(400);
    }




}