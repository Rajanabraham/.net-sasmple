using System.Net;
using System.Net.Http.Json;
using System.Text;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using ApiSampleForSDLC.Models;
using Xunit;

namespace ApiSampleForSDLC.Tests;

public class EmployeesControllerPatchTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public EmployeesControllerPatchTests(WebApplicationFactory<Program> factory)
    {
        // Use the test server's client
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task PatchEmployee_UpdatesSelectedFields_ReturnsNoContentAndPersistsChange()
    {
        // Arrange: create a new employee via POST
        var newEmployee = new Employee
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            Phone = "123-456-7890",
            Salary = 60000
        };
        var postResponse = await _client.PostAsJsonAsync("/api/Employees", newEmployee);
        postResponse.EnsureSuccessStatusCode();
        var createdEmployee = await postResponse.Content.ReadFromJsonAsync<Employee>();
        Assert.NotNull(createdEmployee);
        var employeeId = createdEmployee!.Id;

        // Build a JsonPatchDocument that changes the Salary only
        var patchDoc = new JsonPatchDocument<EmployeePatchDto>();
        patchDoc.Replace(e => e.Salary, 75000);
        var serializedPatch = JsonConvert.SerializeObject(patchDoc);
        var content = new StringContent(serializedPatch, Encoding.UTF8, "application/json-patch+json");

        // Act: send PATCH request
        var patchResponse = await _client.PatchAsync($"/api/Employees/{employeeId}", content);

        // Assert: response should be 204 NoContent
        Assert.Equal(HttpStatusCode.NoContent, patchResponse.StatusCode);

        // Verify that the employee was updated in the database
        var getResponse = await _client.GetAsync($"/api/Employees/{employeeId}");
        getResponse.EnsureSuccessStatusCode();
        var updatedEmployee = await getResponse.Content.ReadFromJsonAsync<Employee>();
        Assert.NotNull(updatedEmployee);
        Assert.Equal(75000, updatedEmployee!.Salary);
        // Ensure other fields stayed the same
        Assert.Equal("John", updatedEmployee.FirstName);
        Assert.Equal("Doe", updatedEmployee.LastName);
    }
}
