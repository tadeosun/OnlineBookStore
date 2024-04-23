using EBookStore.Models;
using EBookStore.RequestDto;
using EBookStore.ResponseDto;

namespace EBookStore.Interfaces
{
    public interface IInventoryService
    {
        Task<InventoryResponse> GetInventory(int id);
        Task<InventoryResponse> CreateInventory(InventoryRequestDto request, string loggedinUser);
        Task<InventoryResponse> UpdateInventory(InventoryRequestDto request, string loggedinUser);
        Task<IList<Books>> SearchInventory(string searchTerm);
        Task<IList<Books>> GetAllInventoryAsync();

    }
}