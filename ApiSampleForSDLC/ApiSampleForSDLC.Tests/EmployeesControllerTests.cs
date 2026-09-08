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
                Department = "Cloud & AI Architecture"
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
            Assert.Equal("Rajan", updated.Name); // Unmodified field remains intact
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
    }
}
