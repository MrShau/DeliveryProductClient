using Avalonia.Platform.Storage;
using Avalonia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DeliveryProductMobile.Services
{
    class LocalStorage
    {
        private static readonly string FilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "token.txt");

        public static async Task SaveTokenAsync(string token)
        {
            await File.WriteAllTextAsync(FilePath, token);
        }

        public static async Task<string?> LoadTokenAsync()
        {
            return File.Exists(FilePath) ? await File.ReadAllTextAsync(FilePath) : null;
        }

        public static void ClearToken()
        {
            if (File.Exists(FilePath))
                File.Delete(FilePath);
        }
    }
}
