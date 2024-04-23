using EBookStore.Data;
using EBookStore.Models;
using EBookStore.RepositoryImplementation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBookStore.Tests.Repository
{
    public class UserRepositoryTests
    {
        private readonly DbContextOptions<DataContext> _dbContextOptions;

        public UserRepositoryTests()
        {
            // Set up the options for the in-memory database
            _dbContextOptions = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "DefaultConnection")
                .Options;
        }
        [Fact]
        public async Task FindUsersByUsernameAsync_ReturnsCorrectUsers()
        {
            // Arrange
            using var context = new DataContext(_dbContextOptions);
            context.Users.Add(new Users { Username = "testuser1", ID = 1 });
            context.Users.Add(new Users { Username = "testuser2", ID = 2 });
            context.SaveChanges();

            var repository = new UserRepository(context);

            // Act
            var result = await repository.GetUserByUsernameAsync("testuser1");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("testuser1", result?.Username);
        }
    }
}
