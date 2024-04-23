using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBookStore.Models
{
    public class Orders
    {
        public long ID { get; set; }
        public string? Username { get; set; }
        public bool IsPaid { get; set; }
        public int? TotalQuantity { get; set; }
        [Precision(18, 2)]
        public decimal? TotalAmount { get; set; }
        public DateTime DateOrdered { get; set;}
    }
}
