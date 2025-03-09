using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using DeliveryProductMobile.ViewModels;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace DeliveryProductMobile;

public partial class ChatView : UserControl
{
    private readonly ChatViewModel _viewModel;

    public ChatView()
    {
        InitializeComponent();
    }

    public ChatView(int orderId)
    {
        _viewModel = new ChatViewModel(ScrollViewerMessages);
        DataContext = _viewModel;
        _ = _viewModel.Load(orderId);
        InitializeComponent();
        _viewModel.ScrollViewerMessages = ScrollViewerMessages;
        AdjustForKeyboard(this, TextBoxMessage);

        Loaded += ChatView_Loaded;
        TextBlockOrderId.Text = $"Заказ № {orderId}";
    }

    private async void ChatView_Loaded(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        await Task.Delay(1000);
        ScrollViewerMessages.ScrollToEnd();
        await Task.Delay(3000);
        ScrollViewerMessages.ScrollToEnd();
    }

    private async void Button_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        TextBlockOrderId.Text = _viewModel.Val.ToString();

        if (!string.IsNullOrWhiteSpace(TextBoxMessage.Text))
        {
            await _viewModel.SendMessage(TextBoxMessage.Text);
            TextBoxMessage.Text = string.Empty;
        }
    }


    public void AdjustForKeyboard(UserControl userControl, Control textBox)
    {
        textBox.GotFocus += (s, e) =>
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Create("ANDROID")))
            {
                Dispatcher.UIThread.Post(() =>
                {
                    BorderInput.Height = 360; // Поднимаем UserControl
                });
            }
        };

        textBox.LostFocus += (s, e) =>
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Create("ANDROID")))
            {
                Dispatcher.UIThread.Post(() =>
                {
                    BorderInput.Height = 82; // Возвращаем обратно
                });
            }
        };
    }

    private async void Button_Click_1(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var topLevel = TopLevel.GetTopLevel(this);

        var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Выберите файл",
            AllowMultiple = false,
            FileTypeFilter = new[]
        {
            new FilePickerFileType("Изображения")
            {
                Patterns = new[] { "*.png", "*.jpg", "*.jpeg" },
                MimeTypes = new[] { "image/png", "image/jpeg" }
            }
        }

        });


        if (files.Count >= 1)
        {
            var file = files.First();
            await _viewModel.SendImage(file);
        }
    }
}