using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using DeliveryProductMobile.Services;
using DeliveryProductMobile.Sessions;
using DeliveryProductMobile.ViewModels;
using DeliveryProductMobile.Views;
using Material.Dialog;
using MsBox.Avalonia;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryProductMobile;

public partial class ProductDetailsView : UserControl
{
    private ProductDetailsViewModel _viewModel;

    public ProductDetailsView()
    {
        InitializeComponent();
    }
    public ProductDetailsView(int id)
    {
        _viewModel = new();
        DataContext = _viewModel;
        _ = _viewModel.LoadDetails(id);
        InitializeComponent();
    }

    private void Button_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        NavigationService.GoBack();
    }

    private async void Button_Click_1(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (_viewModel.Product == null)
            return;

        if (UserSession.Basket.FirstOrDefault(b => b.Product.Id == _viewModel.Product.Id) == null && _viewModel.Product.Count > 0)
        {
            UserSession.Basket.Add(new() { Product = _viewModel.Product, Count = 1 });
            await Config.ShowMessage("Товар добавлен в корзину");
        }
        else
        {
            if (_viewModel.Product.Count > UserSession.Basket.FirstOrDefault(b => b.Product.Id == _viewModel.Product.Id).Count)
            {
                UserSession.Basket.FirstOrDefault(b => b.Product.Id == _viewModel.Product.Id).Count++;
                await Config.ShowMessage("Товар добавлен в корзину (в количестве: " + UserSession.Basket.FirstOrDefault(b => b.Product.Id == _viewModel.Product.Id).Count + " )");
            }
        }

        NavigationService.GoBack();
    }

    
}