using ApiSampleForSDLC.Controllers;
using ApiSampleForSDLC.Models;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace ApiSampleForSDLC.Tests
{
    public class EmployeesControllerTests
    {
        [Fact]
        public void Patch_ExistingEmployee_ReturnsOkWithUpdatedFields()
        {
            // Arrange
            var controller = new EmployeesController();
            var patchDto = new EmployeePatchDto
            {
                Salary = 75000,
                Department = "Cloud & AI Architecture",
                Email = "rajan.updated@example.com"
            };

            // Act
            var result = controller.Patch(1, patchDto) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            var updated = result.Value as Employee;
            Assert.NotNull(updated);
            Assert.Equal(75000, updated.Salary);
            Assert.Equal("Cloud & AI Architecture", updated.Department);
            Assert.Equal("rajan.updated@example.com", updated.Email);
        }

        [Fact]
        public void Patch_NonExistentEmployee_ReturnsNotFound()
        {
            // Arrange
            var controller = new EmployeesController();
            var patchDto = new EmployeePatchDto { Name = "Unknown Employee" };

            // Act
            var result = controller.Patch(9999, patchDto) as NotFoundObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public void Patch_NullPayload_ReturnsBadRequest()
        {
            // Arrange
            var controller = new EmployeesController();

            // Act
            var result = controller.Patch(1, null!) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public void Update_WithEmailField_ReturnsSuccess()
        {
            // Arrange
            var controller = new EmployeesController();
            var employee = new Employee
            {
                Id = 1,
                Name = "Rajan",
                Department = ".NET",
                Salary = 60000,
                Email = "rajan.developer@example.com"
            };

            // Act
            var result = controller.Update(1, employee) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            var updated = result.Value as Employee;
            Assert.NotNull(updated);
            Assert.Equal("rajan.developer@example.com", updated.Email);
        }
    }
}
