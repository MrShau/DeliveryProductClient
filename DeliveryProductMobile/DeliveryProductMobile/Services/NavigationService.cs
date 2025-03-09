using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryProductMobile.Services
{
    class NavigationService
    {
        public static Window? MainWindow;
        private static ContentControl? _frame;
        public static UserControl? PreviosFrame;

        public static void Initialize(ContentControl frame)
        {
            _frame = frame;
        }

        public static void Navigate(UserControl page)
        {

            if (_frame != null)
                _frame.Content = page;
            else
            {
                if (Application.Current?.ApplicationLifetime is ISingleViewApplicationLifetime app)
                {
                    app.MainView = page;
                }
            }
        }

        public static void GoBack()
        {
            if (PreviosFrame != null && _frame != null)
                _frame.Content = PreviosFrame;
        }
    }
}
