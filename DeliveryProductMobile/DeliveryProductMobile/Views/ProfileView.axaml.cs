using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using DeliveryProductMobile.Services;
using DeliveryProductMobile.Sessions;

namespace DeliveryProductMobile;

public partial class ProfileView : UserControl
{
    public ProfileView()
    {
        InitializeComponent();

        TextBlockLogin.Text = UserSession.User?.Login ?? "null";
        TextBlockEmail.Text = UserSession.User?.Email ?? "null";
        TextBlockDate.Text = $"Зарегистрирован с: {UserSession.User?.CreatedAtString}";
    }

    private async void Button_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        NavigationService.Navigate(new OrdersView());
    }

    private void Button_Click_1(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        LocalStorage.ClearToken();
        if (Application.Current?.ApplicationLifetime is ISingleViewApplicationLifetime app)
        {
            app.MainView = new SignInView();
        }
    }

    private void Button_Click_2(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        NavigationService.PreviosFrame = this;
        NavigationService.Navigate(new EditProfileView());
    }
}
