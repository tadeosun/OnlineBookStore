using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBookStore.Models
{
    public class Books
    {
        public long ID { get; set; }
        public string? Title { get; set; }
        public string? Genre { get; set; }
        public string? ISBN { get; set; }
        public string? Author { get; set; }
        [Precision(18, 2)]
        public decimal Price { get; set; }
        public int YearOfPublication { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public string? CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }
        public string? InStock { get; set; }
        public int Quantity { get; set; }
    }
}
