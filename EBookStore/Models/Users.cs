using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBookStore.Models
{
    public class Users
    {
        public long ID { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? MobileNumber { get; set; }
        public string? PasswordHash { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public string? CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }
        public string? Role { get; set; }

    }
}
