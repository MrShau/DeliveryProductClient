using Avalonia.Media.Imaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryProductMobile
{
    public static class ImageLoader
    {
        private static readonly HttpClient _httpClient = new();

        public static async Task<Bitmap?> LoadImageFromUrl(string imageUrl)
        {
            try
            {
                var handler = new HttpClientHandler();
                handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
                using HttpClient client = new(handler);
                var response = await client.GetAsync(imageUrl);
                response.EnsureSuccessStatusCode();
                await using var stream = await response.Content.ReadAsStreamAsync();
                return new Bitmap(stream);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
