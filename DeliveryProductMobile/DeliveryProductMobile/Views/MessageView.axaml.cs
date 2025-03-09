using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.VisualTree;

namespace DeliveryProductMobile;

public partial class MessageView : Window
{
    public MessageView(string message)
    {
        InitializeComponent();
        this.FindControl<TextBlock>("MessageText").Text = message;
    }

    private void CloseDialog(object? sender, RoutedEventArgs e)
    {
        ((Window)this.GetVisualRoot())?.Close();
    }
}