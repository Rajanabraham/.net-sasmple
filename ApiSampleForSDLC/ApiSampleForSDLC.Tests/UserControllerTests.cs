using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ApiSampleForSDLC.Models;
using ApiSampleForSDLC.Models.DTOs;
using Xunit;

namespace ApiSampleForSDLC.Tests;

public class UserControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public UserControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                // Replace the real DB context with an in‑memory version for testing
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<EmployeeContext>));
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                services.AddDbContext<EmployeeContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryUserDb");
                });
            });
        });
    }

    [Fact]
    public async Task GetUsers_ReturnsOk_WithEmptyList()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/User");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var users = await response.Content.ReadFromJsonAsync<List<UserDto>>();
        Assert.NotNull(users);
        Assert.Empty(users);
    }

    [Fact]
    public async Task CreateUser_ReturnsCreatedAndCanBeFetched()
    {
        // Arrange
        var client = _factory.CreateClient();
        var createDto = new CreateUserDto
        {
            UserName = "testuser",
            Email = "test@example.com",
            PasswordHash = "hashedpw"
        };

        // Act - Create
        var postResponse = await client.PostAsJsonAsync("/api/User", createDto);
        Assert.Equal(HttpStatusCode.Created, postResponse.StatusCode);
        var createdUser = await postResponse.Content.ReadFromJsonAsync<UserDto>();
        Assert.NotNull(createdUser);
        Assert.Equal(createDto.UserName, createdUser!.UserName);
        Assert.Equal(createDto.Email, createdUser.Email);

        // Act - Retrieve
        var getResponse = await client.GetAsync($"/api/User/{createdUser.Id}");
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
        var fetchedUser = await getResponse.Content.ReadFromJsonAsync<UserDto>();
        Assert.NotNull(fetchedUser);
        Assert.Equal(createdUser.Id, fetchedUser!.Id);
    }

    [Fact]
    public async Task UpdateUser_ReturnsNoContent_AndUpdatesFields()
    {
        // Arrange
        var client = _factory.CreateClient();
        var createDto = new CreateUserDto
        {
            UserName = "original",
            Email = "original@example.com",
            PasswordHash = "origpw"
        };
        var postResponse = await client.PostAsJsonAsync("/api/User", createDto);
        var createdUser = await postResponse.Content.ReadFromJsonAsync<UserDto>();
        Assert.NotNull(createdUser);

        var updateDto = new UpdateUserDto
        {
            UserName = "updated",
            Email = "updated@example.com"
        };

        // Act
        var putResponse = await client.PutAsJsonAsync($"/api/User/{createdUser!.Id}", updateDto);
        Assert.Equal(HttpStatusCode.NoContent, putResponse.StatusCode);

        // Verify update
        var getResponse = await client.GetAsync($"/api/User/{createdUser.Id}");
        var updatedUser = await getResponse.Content.ReadFromJsonAsync<UserDto>();
        Assert.NotNull(updatedUser);
        Assert.Equal("updated", updatedUser!.UserName);
        Assert.Equal("updated@example.com", updatedUser.Email);
    }

    [Fact]
    public async Task DeleteUser_ReturnsNoContent_AndUserIsGone()
    {
        // Arrange
        var client = _factory.CreateClient();
        var createDto = new CreateUserDto
        {
            UserName = "tobedeleted",
            Email = "delete@example.com",
            PasswordHash = "deletepw"
        };
        var postResponse = await client.PostAsJsonAsync("/api/User", createDto);
        var createdUser = await postResponse.Content.ReadFromJsonAsync<UserDto>();
        Assert.NotNull(createdUser);

        // Act - Delete
        var deleteResponse = await client.DeleteAsync($"/api/User/{createdUser!.Id}");
        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

        // Verify deletion
        var getResponse = await client.GetAsync($"/api/User/{createdUser.Id}");
        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
    }
}
