using EBookStore.ResponseDto;

namespace EBookStore.RequestDto
{
    public class CheckOutRequestDto
    {
        public ShoppingCart? Cart { get; set; }
        public string? PaymentOption { get; set; }
    }
}
