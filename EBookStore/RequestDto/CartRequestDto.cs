using EBookStore.Models;
using EBookStore.ResponseDto;

namespace EBookStore.RequestDto
{
        public class ShoppingCartRequest
        {
            public int ItemID { get; set; }

            public string? Username { get; set; }
        }

}
