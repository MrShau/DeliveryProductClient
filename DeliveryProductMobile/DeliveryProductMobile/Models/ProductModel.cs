using Avalonia.Media.Imaging;
using DeliveryProductMobile.Sessions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryProductMobile.Models
{
    public class ProductModel
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public string? Description { get; set; }

        public double Price { get; set; }

        public int? CategoryId { get; set; }

        public string? ImagePath { get; set; }
        public int? Weight { get; set; }
        public int Count { get; set; }
        public string? WeightUnit { get; set; }
        public bool? InBasket { get => UserSession.Basket.FirstOrDefault(b => b.Product.Id == Id) != null; }

        public Bitmap? Image { get; set; }
    }
}
