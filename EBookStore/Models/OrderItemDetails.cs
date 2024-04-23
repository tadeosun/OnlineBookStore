using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBookStore.Models
{
    public class OrderItemDetails
    {
        public long ID { get; set; }
        public long OrderID { get; set; }
        public long BookID { get; set; }
        [Precision(18, 2)]
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public DateTime OrderDateTime { get; set; }
    }
}
