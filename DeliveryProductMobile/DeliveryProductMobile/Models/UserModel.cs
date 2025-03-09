using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryProductMobile.Models
{
    class UserModel
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string? Email { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedAtString { get => CreatedAt.ToString("dd.MM.yyyy"); }
        public string Role { get; set; }
    }
}
