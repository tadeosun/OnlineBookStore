using Castle.Components.DictionaryAdapter.Xml;
using EBookStore.Common;
using EBookStore.Implementations;
using EBookStore.Interfaces;
using EBookStore.Models;
using EBookStore.RepositoryInterfaces;
using EBookStore.RequestDto;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBookStore.Tests.Implementation
{
    public class InventoryServiceTests
    {
        private Mock<IInventoryRepository> mockRepository;
        private InventoryService service;
        private Mock<IConfigurationAccessor> mockConfig;

        public InventoryServiceTests()
        {
            mockConfig = new Mock<IConfigurationAccessor>();
            mockRepository = new Mock<IInventoryRepository>();
            service = new InventoryService(mockRepository.Object, mockConfig.Object);
        }

        [Fact]
        public async Task GetInventoryById_WhenInventoryExists_ReturnsValidInventory()
        {
            //arrange
            var expectedUser = new Books
            {
                ID = 4,
                Title = "Pride and Prejudice",
                Author = "Jane Austen",
                YearOfPublication = 1813,
                Price = 9
            };
            mockRepository.Setup(x => x.GetInventoryByIdAsync(4)).ReturnsAsync(expectedUser);
            //act
            var result = await service.GetInventory(4);
            ///assert
            Assert.NotNull(result);
            Assert.Equal("Success", result.ResponseMessage);
            Assert.Equal("Pride and Prejudice", result.BookDetails.Title);
        }

        [Fact]
        public async Task GetInventoryById_WhenInventoryDoesNotExist_ReturnsErrorResponse()
        {
            //Arrange
            mockRepository.Setup(x => x.GetInventoryByIdAsync(It.IsAny<int>())).ReturnsAsync((Books)null);

            //act
            var result = await service.GetInventory(1);

            //assert
            Assert.NotNull(result);
            Assert.Equal("Book not found.", result.ResponseMessage);
        }

        [Fact]
        public async Task AddInventory_WhenInventoryIsValid_AddBook()
        {
            //Arrange
            string loggeduser = "admin1";
            mockRepository.Setup(x => x.GetBookByTitleAsync(It.IsAny<string>())).ReturnsAsync((Books)null);
            var genreObj = "Friction,Thriller,Mystery,Poetry,Horror,Satire";
            mockConfig.Setup(m => m.GetValue<string>("GENRES")).Returns(genreObj);
            var book = new InventoryRequestDto
            {
                Title = "PrideandPrejudice",
                Author = "Jane Austen",
                YearOfPublication = 1813,
                Price = 9,
                ISBN = "000-0000",
                Genre = "Thriller",
                InStock = Constants.Yes,
                Quantity = 3
            };

            //Act
            var result = await service.CreateInventory(book, loggeduser);

            //Assert
            this.mockRepository.Verify(repository => repository.AddInventoryAsync(It.IsAny<Books>()), Times.Once);
        }

        [Fact]
        public async Task UpdateInventoryAsync_WhenBookExists_ReturnsTrue()
        {
            // Arrange
            string loggeduser = "admin1";
            var genreObj = "Friction,Thriller,Mystery,Poetry,Horror,Satire";
            mockConfig.Setup(m => m.GetValue<string>("GENRES")).Returns(genreObj);
            var expectedBook = new Books
            {
                Title = "PrideandPrejudice",
                Author = "Jane Austen",
                YearOfPublication = 1813,
                Price = 9,
                ISBN = "000-0000",
                Genre = "Thriller",
                InStock = Constants.Yes,
                Quantity = 3
            };

            var invDTO = new InventoryRequestDto
            {
                Title = "PrideandPrejudice",
                Author = "Jane Austen",
                YearOfPublication = 1813,
                Price = 9,
                ISBN = "000-0000",
                Genre = "Thriller",
                InStock = Constants.Yes,
                Quantity = 3
            };

            mockRepository.Setup(x => x.GetBookByTitleAsync(It.IsAny<string>())).ReturnsAsync(expectedBook);
            mockRepository.Setup(x => x.UpdateInventoryAsync(It.IsAny<Books>())).ReturnsAsync(1);

            // Act
            await service.UpdateInventory(invDTO, loggeduser);

            // Assert
            this.mockRepository.Verify(repository => repository.UpdateInventoryAsync(It.IsAny<Books>()), Times.Once);

        }


    }
}
