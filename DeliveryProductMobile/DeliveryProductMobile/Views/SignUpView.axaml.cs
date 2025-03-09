using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using DeliveryProductMobile.API;
using DeliveryProductMobile.Services;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace DeliveryProductMobile;

public partial class SignUpView : UserControl
{
    private readonly UserApi _userApi;

    public SignUpView()
    {
        _userApi = new();
        InitializeComponent();

        AdjustForKeyboard(this, TextBoxPassword);
        AdjustForKeyboard(this, TextBoxRepeatPassword);
    }

    public void AdjustForKeyboard(UserControl userControl, Control textBox)
    {
        textBox.GotFocus += (s, e) =>
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Create("ANDROID")))
            {
                Dispatcher.UIThread.Post(() =>
                {
                    userControl.Margin = new Thickness(0, -260, 0, 0);// Поднимаем UserControl
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
            TextBlockError.Text = "Некорректный логин ! (длина логин должна быть: 3 - 64 симв.)";
            return;
        }

        if (TextBoxEmail.Text == null || !Regex.IsMatch(TextBoxEmail.Text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
        {
            TextBlockError.Text = "Некорректная почта !";
            return;
        }

        if (
            TextBoxPassword.Text == null ||
            TextBoxPassword.Text.Length < 7 || TextBoxPassword.Text.Length > 255
            )

        {
            TextBlockError.Text = "Некорректный пароль (длина пароля должна быть: 7 - 255 симв.) !";
            return;
        }

        if (
            TextBoxRepeatPassword.Text == null ||
            !TextBoxRepeatPassword.Text.Equals(TextBoxPassword.Text)
            )

        {
            TextBlockError.Text = "Пароли не совпадают !";
            return;
        }

        TextBlockError.Text = await _userApi.SignUp(TextBoxLogin.Text, TextBoxEmail.Text, TextBoxPassword.Text);
    }

    private void Button_Click_1(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (Application.Current?.ApplicationLifetime is ISingleViewApplicationLifetime app)
        {
            app.MainView = new SignInView();
        }
    }
}