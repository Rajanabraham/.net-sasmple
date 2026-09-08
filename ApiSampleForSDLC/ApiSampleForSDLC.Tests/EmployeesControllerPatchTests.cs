using System.Net;
using System.Net.Http.Json;
using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ApiSampleForSDLC.Models;
using Xunit;

namespace ApiSampleForSDLC.Tests;

public class EmployeesControllerPatchTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public EmployeesControllerPatchTests(WebApplicationFactory<Program> factory)
    {
        // The factory creates an in‑memory test server.
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Patch_Updates_Employee_Name()
    {
        // 1️⃣ Arrange – create a baseline employee.
        var newEmployee = new Employee
        {
            Name = "John Doe",
            Age = 30,
            Salary = 50000m
        };
        var postResponse = await _client.PostAsJsonAsync("/api/employees", newEmployee);
        Assert.Equal(HttpStatusCode.Created, postResponse.StatusCode);
        var created = await postResponse.Content.ReadFromJsonAsync<Employee>();
        Assert.NotNull(created);
        var employeeId = created!.Id;

        // 2️⃣ Act – send a JSON Patch document that changes the name.
        var patchOps = new[]
        {
            new { op = "replace", path = "/name", value = "Jane Smith" }
        };
        var patchContent = new StringContent(JsonConvert.SerializeObject(patchOps), Encoding.UTF8, "application/json-patch+json");
        var patchResponse = await _client.PatchAsync($"/api/employees/{employeeId}", patchContent);

        // 3️⃣ Assert – response is 204 No Content and the change persisted.
        Assert.Equal(HttpStatusCode.NoContent, patchResponse.StatusCode);

        var getResponse = await _client.GetAsync($"/api/employees/{employeeId}");
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
        var updated = await getResponse.Content.ReadFromJsonAsync<Employee>();
        Assert.NotNull(updated);
        Assert.Equal("Jane Smith", updated!.Name);
        Assert.Equal(newEmployee.Age, updated.Age);
        Assert.Equal(newEmployee.Salary, updated.Salary);
    }
}
