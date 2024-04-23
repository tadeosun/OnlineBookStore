using EBookStore.RequestDto;
using EBookStore.ResponseDto;

namespace EBookStore.Interfaces
{
    public interface ICartService
    {
        Task<ShoppingCart> CreateCart(ShoppingCartRequest request, string loggedinUser);
        Task<ShoppingCart> ViewCart(string username);
        Task<ShoppingCart> RemoveItemFromCart(int id, string loggedInUser);
    }
}
