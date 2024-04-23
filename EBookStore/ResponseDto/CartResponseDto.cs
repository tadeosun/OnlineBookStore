using EBookStore.Models;

namespace EBookStore.ResponseDto
{
    public class ShoppingCartItem
    {
        public long ID { get; set; }
        public Books Book { get; set; }
        public int Quantity { get; set; }
    }

    public class ShoppingCart
    {
        public List<ShoppingCartItem> CartItems { get; set; }
        public decimal? TotalPrice { get; set; }
        public int TotalQuantity { get; set; }
    }

}
