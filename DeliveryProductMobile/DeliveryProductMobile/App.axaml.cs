using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using Avalonia.Markup.Xaml;
using DeliveryProductMobile.ViewModels;
using DeliveryProductMobile.Views;
using DeliveryProductMobile.Services;
using DeliveryProductMobile.API;

namespace DeliveryProductMobile;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override async void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Avoid duplicate validations from both Avalonia and the CommunityToolkit. 
            // More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
            DisableAvaloniaDataAnnotationValidation();
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainViewModel()
            };

        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            string? token = await LocalStorage.LoadTokenAsync();
            if (!string.IsNullOrEmpty(token))
            {
                UserApi userApi = new();
                await userApi.Auth();
            }
            token = await LocalStorage.LoadTokenAsync();
            if (!string.IsNullOrEmpty(token))
            {
                singleViewPlatform.MainView = new MainView();
            }
            else
            {
                singleViewPlatform.MainView = new SignInView
                {
                };
           }
            
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void DisableAvaloniaDataAnnotationValidation()
    {
        // Get an array of plugins to remove
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        // remove each entry found
        foreach (var plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }
}