using DeliveryProductMobile.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryProductMobile.Sessions
{
    class UserSession
    {
        public static UserModel? User { get; set; }
        public static string JWT_TOKEN { get; set; }
        public static List<BasketProductModel> Basket { get; set; } = new();
    }
}
