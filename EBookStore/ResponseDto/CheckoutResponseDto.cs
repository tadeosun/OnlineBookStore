using EBookStore.Models;

namespace EBookStore.ResponseDto
{
    public class Puchasehistory : Response
    {
        public string Id { get; set; }
        public Books BookDetails { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public DateTime DateOrdered { get; set; }

        public long OrderID { get; set; }
    }

}
