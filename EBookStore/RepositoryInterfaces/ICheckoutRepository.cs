using EBookStore.Models;
using EBookStore.ResponseDto;

namespace EBookStore.RepositoryInterfaces
{
    public interface ICheckoutRepository
    {
        Task BulkInsertOrderDetailItemssAsync(List<OrderItemDetails> orderItemDetails);
        Task<int> AddOrdersAsync(Orders order);
        Task AddOrderItemsAsync(OrderItemDetails orderItemDetail);
        Task<Orders> GetOrderByIdAsync(int id);
        Task<List<Orders>> GetOrderByUsernameAsync(string username);
        Task<List<OrderItemDetails>> GetOrderItemDetailsByIdAsync(int id);
    }
}
