using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using DeliveryProductMobile.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DeliveryProductMobile.API
{
    class ChatApi
    {
        private readonly HttpClient _client;

        public ChatApi()
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
            _client = new HttpClient(handler);

        }

        public async Task<int> GetId(int orderId)
        {
            try
            {
                _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", await LocalStorage.LoadTokenAsync());
                var response = await _client.GetAsync($"{Config.API_HOST}/api/chat/get_id?orderId={orderId}");
                if (response.IsSuccessStatusCode)
                    return int.Parse(await response.Content.ReadAsStringAsync());
                return 0;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<bool> Add(int orderId)
        {
            try
            {
                _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", await LocalStorage.LoadTokenAsync());
                var response = await _client.PostAsJsonAsync($"{Config.API_HOST}/api/chat/add/{orderId}", new { });

                if (response.IsSuccessStatusCode)
                    return true;
            }
            catch (Exception ex)
            {
            }
            return false;

        }

        public async Task<string> UploadImage(int chatId, IStorageFile file)
        {
            try
            {
                _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", await LocalStorage.LoadTokenAsync());

                using var data = new MultipartFormDataContent();
                Stream fileStream;
                StreamContent streamContent;

                fileStream = await file.OpenReadAsync();
                streamContent = new StreamContent(fileStream);
                streamContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                data.Add(streamContent, "File", file.Name);

                data.Add(new StringContent(chatId.ToString()), "ChatId");

                var response = await _client.PostAsync($"{Config.API_HOST}/api/chat/upload_image", data);

                if (!response.IsSuccessStatusCode)
                    return response.StatusCode.ToString();

                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return "Error";
        }

        /*public static byte[] ConvertBitmapToJpg(Bitmap bitmap, int quality = 80)
        {
            using (MemoryStream inputStream = new MemoryStream())
            using (MemoryStream outputStream = new MemoryStream())
            {
                // Сохранение Bitmap в поток
                bitmap.Save(inputStream);
                inputStream.Position = 0;

                // Конвертация в JPG
                using (Image image = Image.Load(inputStream))
                {
                    var encoder = new JpegEncoder { Quality = quality }; // Качество JPG
                    image.Save(outputStream, encoder);
                }

                return outputStream.ToArray();
            }
        }*/
    }
}

//dotnet publish -f net8.0-android -c Release -p:AndroidPackageFormat=apk
