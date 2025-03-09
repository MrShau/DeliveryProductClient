using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DeliveryProductMobile.API;
using DeliveryProductMobile.Models;
using DeliveryProductMobile.Services;
using DeliveryProductMobile.Sessions;
using Nito.AsyncEx;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DeliveryProductMobile.ViewModels
{
    public partial class ProductViewModel : ViewModelBase
    {
        private readonly ProductApi _productApi = new();
        private readonly CategoryApi _categoryApi = new();
        public UserControl? CurrentFrame { get; set; }

        [ObservableProperty]
        private ObservableCollection<ProductModel> products = new();

        [ObservableProperty]
        private ObservableCollection<CategoryModel> categories = new();

        [ObservableProperty]
        private CategoryModel? selectedCategory;

        public ReactiveCommand<CategoryModel, Unit> SelectCategoryCommand { get; }

        public ProductViewModel()
        {
            SelectCategoryCommand = ReactiveCommand.Create<CategoryModel>(SelectCategory);
        }

        public async Task LoadAsync()
        {
            Categories = new(await _categoryApi.GetCategories());
            if (Categories.Count > 0)
                SelectedCategory = Categories.FirstOrDefault();
            await LoadProducts();
        }

        public async Task LoadProducts()
        {
            Products.Clear();
            var products = await _categoryApi.GetProducts(SelectedCategory?.Id ?? 0);
            foreach (var item in products)
            {
                if (item.Count < 1 || item.CategoryId != SelectedCategory?.Id)
                    continue;
                
                item.Image = await ImageLoader.LoadImageFromUrl($"{Config.API_HOST}{item.ImagePath}");
                
                Products.Add(item);
            }

        }

        public async void SelectCategory(CategoryModel category)
        {
            SelectedCategory = category;
            await LoadProducts();
        }

        public async void AddProductInBasket(ProductModel product)
        {
            if (product == null)
                return;
            if (UserSession.Basket.FirstOrDefault(b => b.Product.Id == product.Id) == null)
            {
                if (product.Count > 0)
                {
                    UserSession.Basket.Add(new() { Product = product, Count = 1 });
                    await Config.ShowMessage("Товар добавлен в корзину");
                }
            }
            else
            {
                if (product.Count > UserSession.Basket.FirstOrDefault(b => b.Product.Id == product.Id).Count)
                {
                   UserSession.Basket.FirstOrDefault(b => b.Product.Id == product.Id).Count++;
                    await Config.ShowMessage("Товар добавлен в корзину (в количестве: " + UserSession.Basket.FirstOrDefault(b => b.Product.Id == product.Id).Count + " )");
                }
            }
        }
        public async Task SelectProduct(ProductModel model)
        {
            if (Products.Contains(model))
            {
                ProductDetailsView view = new(model.Id);
                NavigationService.PreviosFrame = CurrentFrame;
                NavigationService.Navigate(view);
            }
        }

        public async void Search(string query)
        {
            if (string.IsNullOrEmpty(query))
                return;
            Products.Clear();
            var products = await _productApi.Search(query);
            foreach (var item in products)
            {
                item.Image = await ImageLoader.LoadImageFromUrl($"{Config.API_HOST}{item.ImagePath}");
                Products.Add(item);
            }
        }

    }
}
