using EBookStore.Models;

namespace EBookStore.RepositoryInterfaces
{
    public interface IInventoryRepository
    {
        Task<Books?> GetInventoryByIdAsync(int Id);
        Task<IList<Books?>> SearchIBookByIdAsync(string searchTerm);
        Task<Books?> GetInventoryByIdAsync(long Id);

        Task<int> AddInventoryAsync(Books book);

        Task<int> UpdateInventoryAsync(Books book);

        Task DeleteInventoryAsync(Books book);
        Task<Books?> GetBookByTitleAsync(string title);
        Task<bool> GetBookExistsAsync(string searchTerm);
        Task<IList<Books>> GetAllBooksAsync();
    }
}
