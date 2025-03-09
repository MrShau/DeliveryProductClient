using CommunityToolkit.Mvvm.ComponentModel;
using DeliveryProductMobile.API;
using DeliveryProductMobile.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryProductMobile.ViewModels
{
    public partial class ProductDetailsViewModel : ViewModelBase
    {
        private readonly ProductApi _productApi = new();

        [ObservableProperty]
        private ProductModel? product;

        public ProductDetailsViewModel()
        {

        }

        public async Task LoadDetails(int id)
        {
            var res = await _productApi.Get(id);
            if (res != null)
                res.Image = await ImageLoader.LoadImageFromUrl($"{Config.API_HOST}{res.ImagePath}");
            Product = res;
        }
    }
}
