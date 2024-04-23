using EBookStore.Common;
using EBookStore.Data;
using EBookStore.Interfaces;
using EBookStore.Models;
using EBookStore.RepositoryInterfaces;
using EBookStore.RequestDto;
using EBookStore.ResponseDto;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Reflection.Metadata;

namespace EBookStore.Implementations
{
    public class InventoryService : IInventoryService
    {
        private readonly IInventoryRepository _inventoryRepository;
       // private readonly IMemoryCache _cache;
        private readonly IConfigurationAccessor _configAccessor;

        public InventoryService(IInventoryRepository inventoryRepository, IConfigurationAccessor configAccessor)//IMemoryCache cache,
        {
            //_cache = cache;
            _inventoryRepository = inventoryRepository;
            _configAccessor = configAccessor;
        }

        public async Task<InventoryResponse> CreateInventory(InventoryRequestDto request, string loggedinUser)
        {
            try
            {
                var response = new InventoryResponse();

                bool isBookValid = ValidateBookRequest(request, response);
                if (!isBookValid) { return response; }

                bool isBookExist = await BookExistsAsync(request.Title);
                if (isBookExist)
                {
                    response.ResponseCode = ResponseMapping.ResponseCode06;
                    response.ResponseMessage = string.Format(ResponseMapping.ResponseCode06Message, "Book");
                    return response;
                }

                var insertRequest = new Books()
                {
                    Title = request.Title,
                    ISBN = request.ISBN,
                    Author = request.Author,
                    YearOfPublication = request.YearOfPublication,
                    Genre = request.Genre,
                    Price = request.Price,
                    CreatedBy = loggedinUser,
                    DateCreated = DateTime.Now,
                    DateModified = DateTime.Now,
                    InStock = Constants.Yes
                };

                int rowID = await _inventoryRepository.AddInventoryAsync(insertRequest);
                if (rowID > 0)
                {
                    var bookInfo = new BookResponseDto()
                    {
                        Title = request.Title,
                        ISBN = request.ISBN,
                        Author = request.Author,
                        YearOfPublication = request.YearOfPublication,
                        Genre = request.Genre,
                        Price = request.Price
                    };
                    response.BookDetails  = bookInfo;
                    response.ResponseCode = ResponseMapping.ResponseCode00;
                    response.ResponseMessage = ResponseMapping.ResponseCode00Message;
                }
                else
                {
                    response.ResponseCode = ResponseMapping.ResponseCode09;
                    response.ResponseMessage = string.Format(ResponseMapping.ResponseCode09Message, "Book");
                }

                return response;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<InventoryResponse> GetInventory(int id)
        {
            try
            {
                var response = new InventoryResponse();
                var bookDetails = await _inventoryRepository.GetInventoryByIdAsync(id);
                if (bookDetails != null)
                {
                    var book = new BookResponseDto()
                    {
                        Title = bookDetails.Title,
                        Author = bookDetails.Author,
                        Genre = bookDetails.Genre,
                        ISBN = bookDetails.ISBN,
                        YearOfPublication = bookDetails.YearOfPublication,
                        Price = bookDetails.Price,
                        Quantity = bookDetails.Quantity,
                        InStock = bookDetails.InStock
                    };

                    response.BookDetails = book;
                    response.ResponseCode = ResponseMapping.ResponseCode00;
                    response.ResponseMessage = ResponseMapping.ResponseCode00Message;
                    return response;
                }
                response.ResponseCode = ResponseMapping.ResponseCode07;
                response.ResponseMessage = string.Format(ResponseMapping.ResponseCode07Message, "Book");
                return response;

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IList<Books>> SearchInventory(string searchTerm)
        {
            try
            {
                return await SearchForBooksAsync(searchTerm);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<InventoryResponse> UpdateInventory(InventoryRequestDto request, string loggedinUser)
        {
            try
            {
                var response = new InventoryResponse();

                bool isBookValid = ValidateBookRequest(request, response);
                if (!isBookValid) { return response; }

                Books existingBook = await GetBookDetailsAsync(request.Title);
                if (existingBook == null)
                {
                    response.ResponseCode = ResponseMapping.ResponseCode07;
                    response.ResponseMessage = string.Format(ResponseMapping.ResponseCode07Message, "Book");
                    return response;
                }
                else
                {
                    existingBook.Author = request.Author;
                    existingBook.Title = request.Title;
                    existingBook.ISBN = request.ISBN;
                    existingBook.YearOfPublication = request.YearOfPublication;
                    existingBook.Genre = request.Genre;
                    existingBook.Price = request.Price;
                    existingBook.ModifiedBy = loggedinUser;
                    existingBook.DateModified = DateTime.Now;
                    existingBook.InStock = request.InStock;
                }


                int rowAffected = await _inventoryRepository.UpdateInventoryAsync(existingBook);
                if (rowAffected > 0)
                {
                    var updateddetails = new BookResponseDto()
                    {
                        Title = existingBook.Title,
                        ISBN = existingBook.ISBN,
                        Author = existingBook.Author,
                        YearOfPublication = existingBook.YearOfPublication,
                        Genre = existingBook.Genre,
                        Price = existingBook.Price,
                        InStock = existingBook.InStock
                    };
                    response.BookDetails = updateddetails;
                    response.ResponseCode = ResponseMapping.ResponseCode00;
                    response.ResponseMessage = ResponseMapping.ResponseCode00Message;
                }
                else
                {
                    response.ResponseCode = ResponseMapping.ResponseCode10;
                    response.ResponseMessage = string.Format(ResponseMapping.ResponseCode10Message, "User");
                }

                return response;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private bool ValidateBookRequest(InventoryRequestDto request, Response response)
        {
            bool isrequestvalid = true;
            if (request == null || (string.IsNullOrEmpty(request.Title)
                                || string.IsNullOrEmpty(request.Author)
                                || string.IsNullOrEmpty(request.ISBN)
             || request.Price <= 0 || string.IsNullOrEmpty(request.Genre)
                                || request.YearOfPublication <= 0))
            {
                response.ResponseCode = ResponseMapping.ResponseCode02;
                response.ResponseMessage = ResponseMapping.ResponseCode02Message;
                isrequestvalid = false;
                return isrequestvalid;
            }

            if (!Helper.IsValidTitle(request.Title))
            {
                response.ResponseCode = ResponseMapping.ResponseCode04;
                response.ResponseMessage = ResponseMapping.ResponseCode04Message;
                isrequestvalid = false;
                return isrequestvalid;
            }

            if (!Helper.IsValidISBN(request.ISBN))
            {
                response.ResponseCode = ResponseMapping.ResponseCode05;
                response.ResponseMessage = ResponseMapping.ResponseCode05Message;
                isrequestvalid = false;
                return isrequestvalid;
            }

            if (!isGenreInAllowedList(request.Genre))
            {
                response.ResponseCode = ResponseMapping.ResponseCode03;
                response.ResponseMessage = ResponseMapping.ResponseCode03Message;
                isrequestvalid = false;
                return isrequestvalid;
            }
            return isrequestvalid;
        }

        private async Task<bool> BookExistsAsync(string searchTerm)
        {
            return await _inventoryRepository.GetBookExistsAsync(searchTerm);
        }

        private async Task<Books?> GetBookDetailsAsync(string searchTerm)
        {
            return await _inventoryRepository.GetBookByTitleAsync(searchTerm);
        }

        private async Task<IList<Books?>> SearchForBooksAsync(string searchTerm)
        {
            return await _inventoryRepository.SearchIBookByIdAsync(searchTerm);
        }

        public async Task<IList<Books>> GetAllInventoryAsync()
        {
            try
            {
                return await _inventoryRepository.GetAllBooksAsync();
            }
            catch (Exception ex) { throw; }
        }

        private bool isGenreInAllowedList(string genre)
        {
            var genres = _configAccessor.GetValue<string>("GENRES").Split(',');

            return genres.Contains(genre);
        }
    }
}
