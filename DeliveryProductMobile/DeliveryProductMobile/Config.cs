using Avalonia.Controls;
using MsBox.Avalonia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DeliveryProductMobile
{
    class Config
    {
        //public const string API_HOST = "https://10.0.2.2:7002";
        //public const string API_HOST = "http://DeliveryProductAPI.somee.com";
        public const string API_HOST = "https://deliveryproductapi.bsite.net";
        public static bool IsEmail(string text) => Regex.IsMatch(text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");

        public static async Task ShowMessage(string text)
        {
            await MessageBoxManager.GetMessageBoxStandard("Информация", text).ShowAsync();
        }
    }
}
