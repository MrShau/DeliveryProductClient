using DeliveryProductMobile.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DeliveryProductMobile.API
{
    class CategoryApi
    {
        private readonly HttpClient _client;

        public CategoryApi()
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
            _client = new HttpClient(handler);
        }

        public async Task<List<ProductModel>> GetProducts(int categoryId)
        {
            try
            {
                var response = await _client.GetAsync($"{Config.API_HOST}/api/category/get_products?categoryId={categoryId}");

                if (!response.IsSuccessStatusCode)
                    return new();

                string responseString = await response.Content.ReadAsStringAsync();

                return Newtonsoft.Json.JsonConvert.DeserializeObject<List<ProductModel>>(responseString) ?? new();
            }
            catch (Exception e)
            {
            }

            return new();
        }

        public async Task<List<CategoryModel>> GetCategories()
        {
            try
            {
                var response = await _client.GetAsync($"{Config.API_HOST}/api/category/get_all");

                if (!response.IsSuccessStatusCode)
                    return new();

                string responseString = await response.Content.ReadAsStringAsync();

                return Newtonsoft.Json.JsonConvert.DeserializeObject<List<CategoryModel>>(responseString) ?? new();
            }
            catch (Exception e)
            {
            }

            return new();
        }

    }
}
