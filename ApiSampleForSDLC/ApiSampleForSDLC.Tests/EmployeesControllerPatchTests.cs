using System.Net;
using System.Net.Http.Json;
using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ApiSampleForSDLC;
using ApiSampleForSDLC.Models;
using Xunit;

namespace ApiSampleForSDLC.Tests;

public class EmployeesControllerPatchTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public EmployeesControllerPatchTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                // Replace the real DbContext with an in‑memory version for testing
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<EmployeeDbContext>));
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }
                services.AddDbContext<EmployeeDbContext>(options =>
                    options.UseInMemoryDatabase("InMemoryEmployeeTestDb"));
            });
        });
    }

    [Fact]
    public async Task PatchEmployee_UpdatesName_ReturnsNoContent()
    {
        // Arrange: seed an employee
        using (var scope = _factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<EmployeeDbContext>();
            db.Employees.Add(new Employee { Id = 1, Name = "John Doe", Age = 30, Position = "Developer" });
            db.SaveChanges();
        }

        var client = _factory.CreateClient();

        // Build JSON Patch document: replace Name
        var patchDoc = new[]
        {
            new { op = "replace", path = "/name", value = "Jane Smith" }
        };
        var json = JsonConvert.SerializeObject(patchDoc);
        var content = new StringContent(json, Encoding.UTF8, "application/json-patch+json");

        // Act
        var response = await client.PatchAsync("/api/Employees/1", content);

        // Assert status code
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        // Verify the change persisted
        using (var scope = _factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<EmployeeDbContext>();
            var employee = await db.Employees.FindAsync(1);
            Assert.NotNull(employee);
            Assert.Equal("Jane Smith", employee!.Name);
        }
    }
}
