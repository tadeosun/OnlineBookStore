using System.Linq.Expressions;
using EBookStore.Common;
using EBookStore.Data;
using EBookStore.Implementations;
using EBookStore.Interfaces;
using EBookStore.Models;
using EBookStore.RequestDto;
using EBookStore.ResponseDto;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Moq;


namespace EBookStore.Tests.Implementation
{
    public class UserServiceTests
    {
        private Mock<IUserRepository> mockRepository;
        private UserService service;

        public UserServiceTests()
        {
            mockRepository = new Mock<IUserRepository>();
            service = new UserService(mockRepository.Object);
        }

        [Fact]
        public async Task GetUserById_WhenUserExists_ReturnsValidUser()
        {
            var expectedUser = new Users
            {
                ID = 1,
                Username = "testuser",
                Email = "test@example.com",
                MobileNumber = "1234567890",
            };
            mockRepository.Setup(x => x.GetUserByIdAsync(1)).ReturnsAsync(expectedUser);

            var result = await service.GetUserById(1);

            Assert.NotNull(result);
            Assert.Equal("Success", result.ResponseMessage);
            Assert.Equal("testuser", result.UserDetails.Username);
        }

        [Fact]
        public async Task GetUserById_WhenUserDoesNotExist_ReturnsErrorResponse()
        {
            mockRepository.Setup(x => x.GetUserByIdAsync(It.IsAny<int>())).ReturnsAsync((Users)null);

            var result = await service.GetUserById(1);

            Assert.NotNull(result);
            Assert.Equal("User not found.", result.ResponseMessage);
            //Assert.Equal("User does not exist.", result.ResponseMessage);
        }

        [Fact]
        public async Task AddUser_WhenUserIsValid_AddsUser()
        {
            // Arrange
            string loggeduser = "admin1";
            mockRepository.Setup(x => x.GetUserByUsernameAsync(It.IsAny<string>())).ReturnsAsync((Users)null);
            var user = new UserRequestDto
            {
                Email = "test@example.com",
                Password = "Password1!",
                Username = "testuser",
                MobileNumber = "1234567890",
                Role = "Admin"
            };

            // Act
            var result = await service.AddUser(user, loggeduser);

            // Assert
            this.mockRepository.Verify(repository => repository.AddUserAsync(It.IsAny<Users>()), Times.Once);
        }

        [Fact]
        public async Task UpdateUserAsync_WhenUserExists_ReturnsTrue()
        {
            // Arrange
            string loggeduser = "admin1";
            var expectedUser = new Users
            {
                Email = "test@example.com",
                Username = "testuser",
                MobileNumber = "1234567890",
                Role = "Admin"
            };

            var userDTO = new UserRequestDto
            {
                Email = "testuser@example.com",
                MobileNumber = "1234567890",
                Role = "Admin",
                Password = "Password4!",
                Username = "testuser"
            };

            mockRepository.Setup(x => x.GetUserByUsernameAsync(It.IsAny<string>())).ReturnsAsync(expectedUser);
            mockRepository.Setup(x => x.UpdateUserAsync(It.IsAny<Users>())).ReturnsAsync(1);

            // Act
            await service.UpdateUser(userDTO, loggeduser);

            // Assert
            this.mockRepository.Verify(repository => repository.UpdateUserAsync(It.IsAny<Users>()), Times.Once);

        }

    }
}
