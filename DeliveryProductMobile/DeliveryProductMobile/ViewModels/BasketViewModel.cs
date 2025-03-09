using CommunityToolkit.Mvvm.ComponentModel;
using DeliveryProductMobile.Models;
using DeliveryProductMobile.Sessions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryProductMobile.ViewModels
{
    public partial class BasketViewModel : ViewModelBase
    {
        [ObservableProperty]
        private ObservableCollection<BasketProductModel> basket;

        [ObservableProperty]
        private double totalPrice = 0;

        public BasketViewModel()
        {

        }

        public async Task Load()
        {
            var bask = new List<BasketProductModel>(UserSession.Basket);
            foreach (var product in bask)
                product.Product.Image = await ImageLoader.LoadImageFromUrl($"{Config.API_HOST}{product.Product.ImagePath}");

            Basket = new(bask);

            TotalPrice = Basket.Sum(p => p.Product.Price * p.Count);
        }

        public void AddCount(BasketProductModel basketProduct)
        {
            if (basketProduct != null && basketProduct.Product.Count > UserSession.Basket[Basket.IndexOf(basketProduct)].Count)
            {
                UserSession.Basket[Basket.IndexOf(basketProduct)] = new BasketProductModel()
                {
                    Product = basketProduct.Product,
                    Count = basketProduct.Count + 1
                };
                Basket = new(UserSession.Basket);
                this.OnPropertyChanged(nameof(Basket));
                TotalPrice = Basket.Sum(p => p.Product.Price * p.Count);
            }
        }

        public void MinusCount(BasketProductModel basketProduct)
        {
            if (basketProduct != null && basketProduct.Count > 0)
            {
                if (basketProduct.Count == 1)
                {
                    UserSession.Basket.Remove(basketProduct);
                }
                else
                    UserSession.Basket[Basket.IndexOf(basketProduct)] = new BasketProductModel()
                    {
                        Product = basketProduct.Product,
                        Count = basketProduct.Count - 1
                    };
                Basket = new(UserSession.Basket);
                this.OnPropertyChanged(nameof(Basket));
                TotalPrice = Basket.Sum(p => p.Product.Price * p.Count);
            }
        }
    }
}
