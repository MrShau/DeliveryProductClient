using DeliveryProductMobile.Services;
using DeliveryProductMobile.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryProductMobile.API
{
    class OrderApi
    {
        private readonly HttpClient _client;

        public OrderApi()
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
            _client = new HttpClient(handler);
            
        }

        public async Task Create(int deliveryTime, int deliveryPrice)
        {
            try
            {
                _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", await LocalStorage.LoadTokenAsync());
                List<dynamic> data = new();
                foreach (var item in UserSession.Basket)
                    data.Add(new
                    {
                        ProductId = item.Product.Id,
                        Count = item.Count,
                        
                    });
                if (data.Count < 1)
                    return;
                
                var response = await _client.PostAsJsonAsync($"{Config.API_HOST}/api/order/add", new
                {
                    Products = data,
                    DeliveryTime = deliveryTime,
                    DeliveryPrice = deliveryPrice
                });

                if (response.IsSuccessStatusCode)
                    UserSession.Basket.Clear();

            }
            catch (Exception ex) { }
        }


    }
}
