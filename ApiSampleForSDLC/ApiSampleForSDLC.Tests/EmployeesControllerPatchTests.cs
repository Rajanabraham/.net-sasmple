using System.Net;
using System.Net.Http.Json;
using System.Text;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ApiSampleForSDLC;
using ApiSampleForSDLC.Models;
using Xunit;

namespace ApiSampleForSDLC.Tests;

public class EmployeesControllerPatchTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public EmployeesControllerPatchTests(WebApplicationFactory<Program> factory)
    {
        // Configure the factory to use an in‑memory database for isolation.
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<EmployeeContext>));
                if (descriptor != null) services.Remove(descriptor);

                services.AddDbContext<EmployeeContext>(options =>
                    options.UseInMemoryDatabase("InMemoryEmployeeDb"));

                // Build the service provider.
                var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<EmployeeContext>();
                db.Database.EnsureCreated();

                // Seed a test employee.
                db.Employees.Add(new Employee
                {
                    Id = 1,
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "john.doe@example.com",
                    Position = "Developer",
                    Salary = 80000
                });
                db.SaveChanges();
            });
        });
    }

    [Fact]
    public async Task PatchEmployee_UpdatesFirstName_ReturnsNoContent()
    {
        // Arrange
        var client = _factory.CreateClient();
        var patchDoc = new JsonPatchDocument<EmployeePatchDto>();
        patchDoc.Replace(e => e.FirstName, "Jane");
        var serializedPatch = JsonSerializer.Serialize(patchDoc);
        var content = new StringContent(serializedPatch, Encoding.UTF8, "application/json-patch+json");

        // Act
        var response = await client.PatchAsync("/api/employees/1", content);

        // Assert response status
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        // Verify the employee was updated in the DB.
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<EmployeeContext>();
        var updatedEmployee = await db.Employees.FirstAsync(e => e.Id == 1);
        Assert.Equal("Jane", updatedEmployee.FirstName);
    }
}
