using System.Net;
using System.Net.Http.Json;
using ApiSampleForSDLC.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace ApiSampleForSDLC.Tests
{
    public class EmployeesControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public EmployeesControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task PostEmployee_ShouldPersistAndReturnEmail()
        {
            // Arrange
            var newEmployee = new Employee
            {
                Name = "Jane Doe",
                Age = 30,
                Email = "jane.doe@example.com"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/employees", newEmployee);

            // Assert status code
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            // Deserialize the created employee
            var createdEmployee = await response.Content.ReadFromJsonAsync<Employee>();
            Assert.NotNull(createdEmployee);
            Assert.Equal(newEmployee.Name, createdEmployee!.Name);
            Assert.Equal(newEmployee.Age, createdEmployee.Age);
            Assert.Equal(newEmployee.Email, createdEmployee.Email);
            Assert.True(createdEmployee.Id > 0);
        }
    }
}