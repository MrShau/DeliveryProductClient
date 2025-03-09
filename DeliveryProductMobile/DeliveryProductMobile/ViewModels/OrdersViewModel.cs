using Avalonia.Controls;
using Avalonia.Controls.Converters;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using DeliveryProductMobile.API;
using DeliveryProductMobile.Models;
using DeliveryProductMobile.Services;
using DeliveryProductMobile.Sessions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryProductMobile.ViewModels
{
    public partial class OrdersViewModel : ViewModelBase
    {
        private readonly UserApi _userApi = new();

        public Button? ButtonTabActive;
        public Button? ButtonTabCompleted;

        public UserControl? CurrentView;

        private List<OrderModel> _allOrders;

        [ObservableProperty]
        private ObservableCollection<OrderModel> orders;

        private bool isTabActive = true;

        public async Task Load()
        {
            try
            {
                _allOrders = await _userApi.GetMyOrders();
                Orders = new(_allOrders.Where(o => !o.IsCompleted));
            }
            catch (Exception ex) { }
        }

        public async void SelectTabActive()
        {
            _allOrders = await _userApi.GetMyOrders();
            Orders = new(_allOrders.Where(o => !o.IsCompleted).OrderByDescending(o => o.CreatedAt));
            if (ButtonTabActive != null && ButtonTabCompleted != null)
            {
                ButtonTabActive.Background = new SolidColorBrush(Color.FromRgb(244, 66, 53));
                ButtonTabActive.Foreground = new SolidColorBrush(Colors.WhiteSmoke);

                ButtonTabCompleted.Background = new SolidColorBrush(Colors.WhiteSmoke);
                ButtonTabCompleted.Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        public async void SelectTabCompleted()
        {
            _allOrders = await _userApi.GetMyOrders(false);
            Orders = new(_allOrders.Where(o => o.IsCompleted).OrderByDescending(o => o.CreatedAt));
            if (ButtonTabActive != null && ButtonTabCompleted != null)
            {
                ButtonTabActive.Background = new SolidColorBrush(Colors.WhiteSmoke);
                ButtonTabActive.Foreground = new SolidColorBrush(Colors.Black);

                ButtonTabCompleted.Background = new SolidColorBrush(Color.FromRgb(244, 66, 53));
                ButtonTabCompleted.Foreground = new SolidColorBrush(Colors.WhiteSmoke);
            }
        }

        public async void NavigateToChat(OrderModel model)
        {
            if (Orders.Contains(model))
            {
                if (CurrentView != null)
                    NavigationService.PreviosFrame = CurrentView;

                NavigationService.Navigate(new ChatView(model.Id));
            }
        }
    }
}
