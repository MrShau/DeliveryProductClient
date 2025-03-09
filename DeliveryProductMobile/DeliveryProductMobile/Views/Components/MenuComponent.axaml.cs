using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using DeliveryProductMobile.Services;

namespace DeliveryProductMobile;

public partial class MenuComponent : UserControl
{
    public MenuComponent()
    {
        InitializeComponent();
    }

    private void OnShoppingClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        ProductsView view = new();

        NavigationService.Navigate(view);
    }

    private void OnChatClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
    }

    private void OnBasketClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        BasketView view = new();
        NavigationService.Navigate(view);
    }

    private void OnProfileClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        NavigationService.Navigate(new ProfileView());
    }
}