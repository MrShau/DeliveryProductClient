using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using DeliveryProductMobile.ViewModels;

namespace DeliveryProductMobile;

public partial class OrdersView : UserControl
{
    private readonly OrdersViewModel _viewModel;

    public OrdersView()
    {
        _viewModel = new();
        DataContext = _viewModel;
        _ = _viewModel.Load();
        InitializeComponent();

        _viewModel.CurrentView = this;
        _viewModel.ButtonTabActive = ButtonTabActive;
        _viewModel.ButtonTabCompleted = ButtonTabCompleted;
    }

    private void Button_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        _viewModel.SelectTabActive();
    }

    private void Button_Click_1(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        _viewModel.SelectTabCompleted();

    }
}