using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryProductMobile.Models
{
    public class OrderModel
    {
        public int Id { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; }
        public bool IsCompleted { get; set; }
        public double TotalPrice { get; set; }
        public DateTime CreatedAt { get; set; }
        public double DeliveryPrice { get; set; }
        public int DeliveryTime { get; set; }
        public string? CreatedAtString { get => CreatedAt.ToString("dd.MM.yy HH:mm"); }
        public List<BasketProductModel> Products { get; set; } = new();
    }
}
