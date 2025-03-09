using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using DeliveryProductMobile.API;
using DeliveryProductMobile.Services;
using DeliveryProductMobile.Sessions;
using System.Threading.Tasks;

namespace DeliveryProductMobile;

public partial class EditProfileView : UserControl
{
    private readonly UserApi _userApi = new();

    public EditProfileView()
    {
        InitializeComponent();

        TextBoxLogin.Text = UserSession.User?.Login;
        TextBoxEmail.Text = UserSession.User?.Email;

    }

    private void Button_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        NavigationService.GoBack();
    }

    private async void Button_Click_1(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (TextBoxLogin.Text.Length < 3|| TextBoxLogin.Text.Length > 64)
        {
            TextBlockErrorLogin.Text = "Некорректный логин";
            return;
        }
        var result = await _userApi.ChangeLogin(TextBoxLogin.Text);

        if (result.Contains("SUCCESS"))
        {
            LocalStorage.ClearToken();
            if (Application.Current?.ApplicationLifetime is ISingleViewApplicationLifetime app)
            {
                app.MainView = new SignInView();
            }
            return;
        }

        TextBlockErrorLogin.Text = result;
    }

    private async void Button_Click_2(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (TextBoxEmail.Text.Length < 3 || TextBoxEmail.Text.Length > 255 || !Config.IsEmail(TextBoxEmail.Text))
        {
            TextBlockErrorEmail.Text = "Некорректная почта";
            return;
        }
        var result = await _userApi.ChangeEmail(TextBoxEmail.Text);

        if (result.Contains("SUCCESS"))
        {
            LocalStorage.ClearToken();
            if (Application.Current?.ApplicationLifetime is ISingleViewApplicationLifetime app)
            {
                app.MainView = new SignInView();
            }
            return;
        }

        TextBlockErrorEmail.Text = result;
    }

    private async void Button_Click_3(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (TextBoxPassword.Text.Length < 7 || TextBoxPassword.Text.Length > 255)
        {
            TextBlockErrorPassword.Text = "Некорректный пароль";
            return;
        }
        var result = await _userApi.ChangePassword(TextBoxPassword.Text);

        if (result.Contains("SUCCESS"))
        {
            LocalStorage.ClearToken();
            if (Application.Current?.ApplicationLifetime is ISingleViewApplicationLifetime app)
            {
                app.MainView = new SignInView();
            }
            return;
        }

        TextBlockErrorPassword.Text = result;
    }

    private async void Button_Click_4(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (await _userApi.Delete())
        {
            LocalStorage.ClearToken();
            if (Application.Current?.ApplicationLifetime is ISingleViewApplicationLifetime app)
            {
                app.MainView = new SignInView();
            }
        }
    }
}