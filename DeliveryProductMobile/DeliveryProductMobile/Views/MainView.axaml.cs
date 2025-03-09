using Avalonia.Controls;
using DeliveryProductMobile.Services;

namespace DeliveryProductMobile.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();
        NavigationService.Initialize(this.FindControl<ContentControl>("Frame"));
        NavigationService.Navigate(new ProductsView());
    }


}