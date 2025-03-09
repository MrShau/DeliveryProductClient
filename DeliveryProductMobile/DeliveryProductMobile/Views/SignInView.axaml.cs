using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using DeliveryProductMobile.API;
using DeliveryProductMobile.Services;
using DeliveryProductMobile.ViewModels;
using DeliveryProductMobile.Views;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DeliveryProductMobile;

public partial class SignInView : UserControl
{
    private readonly UserApi _userApi;

    private static readonly Regex LoginRegex =
        new(@"^[a-zA-Z0-9_.-]+$", RegexOptions.Compiled);

    public SignInView()
    {
        InitializeComponent();
        _userApi = new UserApi();
        AdjustForKeyboard(this, TextBoxLogin);
        AdjustForKeyboard(this, TextBoxPassword);
    }

    public void AdjustForKeyboard(UserControl userControl, Control textBox)
    {
        textBox.GotFocus += (s, e) =>
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Create("ANDROID")))
            {
                Dispatcher.UIThread.Post(() =>
                {
                    userControl.Margin = new Thickness(0, -360, 0, 0);// Поднимаем UserControl
                });
            }
        };

        textBox.LostFocus += (s, e) =>
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Create("ANDROID")))
            {
                Dispatcher.UIThread.Post(() =>
                {
                    userControl.Margin = new Thickness(0);
                });
            }
        };
    }

    private async void Button_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (
            TextBoxLogin.Text == null ||
            TextBoxLogin.Text.Length < 3 || TextBoxLogin.Text.Length > 64
            )
        {
            TextBlockError.Text = "Некорректный логин !";
            return;
        }
        if (!Regex.IsMatch(TextBoxLogin.Text, @"^[a-zA-Z0-9_.-]+$"))
        {
            if (!Regex.IsMatch(TextBoxLogin.Text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                TextBlockError.Text = "Некорректный логин или почта";
                return;
            }
        }
        
        if (
            TextBoxPassword.Text == null ||
            TextBoxPassword.Text.Length < 7 || TextBoxPassword.Text.Length > 255
            )

        {
            TextBlockError.Text = "Некорректный пароль !";
            return;
        }

        TextBlockError.Text = await _userApi.SignIn(TextBoxLogin.Text, TextBoxPassword.Text);
    }

    private void Button_Click_1(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (Application.Current?.ApplicationLifetime is ISingleViewApplicationLifetime app)
        {
            app.MainView = new SignUpView();
        }
    }
}