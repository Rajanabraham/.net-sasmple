using ApiSampleForSDLC.Controllers;
using ApiSampleForSDLC.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApiSampleForSDLC.Tests;

public class EmployeesControllerPatchTests
{
    private readonly EmployeesController _controller;

    public EmployeesControllerPatchTests()
    {
        // Fresh controller instance ensures a clean in‑memory store.
        _controller = new EmployeesController();
    }

    [Fact]
    public void Patch_ExistingEmployee_UpdatesOnlyProvidedFields_AndReturnsNoContent()
    {
        // Arrange – patch only the Name of employee with Id = 1.
        var patchDto = new EmployeePatchDto
        {
            Name = "John Updated"
        };

        // Act
        var result = _controller.Patch(1, patchDto);

        // Assert
        Assert.IsType<NoContentResult>(result);
        var updatedEmployee = (_controller.Get(1) as OkObjectResult)?.Value as Employee;
        Assert.NotNull(updatedEmployee);
        Assert.Equal("John Updated", updatedEmployee.Name);
        // Unchanged fields should retain original values.
        Assert.Equal("Developer", updatedEmployee.Position);
        Assert.Equal("Engineering", updatedEmployee.Department);
        Assert.Equal(85000, updatedEmployee.Salary);
    }

    [Fact]
    public void Patch_NonExistingEmployee_ReturnsNotFound()
    {
        // Arrange
        var patchDto = new EmployeePatchDto { Name = "Ghost" };

        // Act
        var result = _controller.Patch(999, patchDto);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void Patch_NullPatchDto_ReturnsBadRequest()
    {
        // Act
        var result = _controller.Patch(1, null!);

        // Assert
        Assert.IsType<BadRequestResult>(result);
    }
}
