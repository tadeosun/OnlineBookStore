using EBookStore.Models;

namespace EBookStore.ResponseDto
{

    public class InventoryResponse : Response
    {
        public BookResponseDto BookDetails { get; set; }
    }

    public class BookResponseDto
    {
        public string? Title { get; set; }
        public string? Genre { get; set; }
        public string? ISBN { get; set; }
        public string? Author { get; set; }
        public int YearOfPublication { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string? InStock { get; set; }
    }


}
