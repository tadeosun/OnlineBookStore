using EBookStore.RequestDto;
using EBookStore.ResponseDto;

namespace EBookStore.Interfaces
{
    public interface ICheckoutService
    {
        Task<Puchasehistory> CheckOut(CheckOutRequestDto request, string loggedinUser);
        Task<IList<Puchasehistory>> ViewPurchaseHistory(string username);
    }
}
