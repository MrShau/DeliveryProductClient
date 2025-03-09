using Avalonia;
using Avalonia.Controls;
using Avalonia.Logging;
using Avalonia.Markup.Xaml;
using DeliveryProductMobile.ViewModels;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace DeliveryProductMobile;

public partial class ProductsView : UserControl
{
    private readonly ProductViewModel _viewModel;
    public ProductsView()
    {
        _viewModel = new ProductViewModel();
        DataContext = _viewModel;
        _viewModel.CurrentFrame = this;
        _ = _viewModel.LoadAsync();
        InitializeComponent();
    }

    private void TextBox_GotFocus(object? sender, Avalonia.Input.GotFocusEventArgs e)
    {
        if (TextBoxSearch.Text == "Введите название нужного товара")
            TextBoxSearch.Text = "";
    }

    private void TextBox_LostFocus(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (TextBoxSearch.Text?.Length < 1)
            TextBoxSearch.Text = "Введите название нужного товара";
    }

    private void Button_Click_1(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (TextBoxSearch.Text?.Length < 1)
            return;

        _viewModel.Search(TextBoxSearch.Text);
    }
}