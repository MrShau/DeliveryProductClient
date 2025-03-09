using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using DeliveryProductMobile.API;
using DeliveryProductMobile.Services;
using DeliveryProductMobile.Sessions;
using DeliveryProductMobile.ViewModels;
using System;

namespace DeliveryProductMobile;

public partial class BasketView : UserControl
{
    private readonly BasketViewModel _viewModel;
    private readonly OrderApi _orderApi;
    private int deliveryPrice = 0;
    private int deliveryTime = 0;

    public BasketView()
    {
        _viewModel = new();
        DataContext = _viewModel;
        _ = _viewModel.Load();

        InitializeComponent();

        _orderApi = new OrderApi();

        deliveryPrice = new Random().Next(200, 320) / 100 * 100;
        deliveryTime = new Random().Next(20, Math.Max(UserSession.Basket.Count * 10 > 35 ? 35 : UserSession.Basket.Count * 10, 22)) / 10 * 10;

        
    }

    private async void Button_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (UserSession.Basket.Count < 1)
            return;

        await _orderApi.Create(deliveryTime, deliveryPrice);
        _viewModel.Basket = new (UserSession.Basket);
        NavigationService.Navigate(new OrdersView());
    }

    private void UserControl_Loaded_1(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        TextBlockDeliveryPrice.Text = $"Стоимость: {deliveryPrice} ₽";
        TextBlockDeliveryTime.Text = $"Длительность: {deliveryTime} минут.";
    }
}