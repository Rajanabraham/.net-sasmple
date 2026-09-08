using System.Net.Http.Json;
using ApiSampleForSDLC.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace ApiSampleForSDLC.Tests;

public class EmployeesControllerEmailTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public EmployeesControllerEmailTests(WebApplicationFactory<Program> factory)
    {
        // Override the DbContext with an in‑memory database for isolated tests
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<EmployeeContext>));
                if (descriptor != null)
                    services.Remove(descriptor);

                services.AddDbContext<EmployeeContext>(options =>
                    options.UseInMemoryDatabase("TestDb"));

                // Ensure the DB is created and seeded with a test employee
                var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();
                var ctx = scope.ServiceProvider.GetRequiredService<EmployeeContext>();
                ctx.Database.EnsureCreated();
                ctx.Employees.Add(new Employee { EmployeeId = 1, FirstName = "John", LastName = "Doe" });
                ctx.SaveChanges();
            });
        });
    }

    [Fact]
    public async Task AddEmail_ReturnsCreatedEmail()
    {
        // Arrange
        var client = _factory.CreateClient();
        var emailDto = new EmailDto
        {
            EmailAddress = "john.doe@example.com",
            IsPrimary = true
        };

        // Act
        var response = await client.PostAsJsonAsync("/api/employees/1/emails", emailDto);

        // Assert
        response.EnsureSuccessStatusCode();
        var createdEmail = await response.Content.ReadFromJsonAsync<Email>();
        Assert.NotNull(createdEmail);
        Assert.Equal(emailDto.EmailAddress, createdEmail!.EmailAddress);
        Assert.True(createdEmail.IsPrimary);
        Assert.Equal(1, createdEmail.EmployeeId);
    }
}
