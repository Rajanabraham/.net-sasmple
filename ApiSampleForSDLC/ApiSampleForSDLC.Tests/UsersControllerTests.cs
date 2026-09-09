using ApiSampleForSDLC.Controllers;
using ApiSampleForSDLC.DTOs;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace ApiSampleForSDLC.Tests
{
    public class UsersControllerTests
    {
        [Fact]
        public void GetAll_Initially_ReturnsEmptyCollection()
        {
            // Arrange
            var controller = new UsersController();

            // Act
            var result = controller.GetAll() as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            var collection = Assert.IsAssignableFrom<IEnumerable<UserDto>>(result!.Value);
            Assert.Empty(collection);
        }

        [Fact]
        public void Create_Then_GetById_ReturnsCreatedUser()
        {
            // Arrange
            var controller = new UsersController();
            var newUser = new UserDto
            {
                Username = "jdoe",
                Email = "jdoe@example.com",
                Role = "Admin"
            };

            // Act - Create
            var createResult = controller.Create(newUser) as CreatedAtActionResult;
            Assert.NotNull(createResult);
            var createdDto = Assert.IsType<UserDto>(createResult!.Value);

            // Act - GetById
            var getResult = controller.GetById(createdDto.Id) as OkObjectResult;
            Assert.NotNull(getResult);
            var fetchedDto = Assert.IsType<UserDto>(getResult!.Value);

            // Assert
            Assert.Equal(createdDto.Id, fetchedDto.Id);
            Assert.Equal("jdoe", fetchedDto.Username);
            Assert.Equal("jdoe@example.com", fetchedDto.Email);
            Assert.Equal("Admin", fetchedDto.Role);
        }
    }
}
