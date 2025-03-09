using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using DeliveryProductMobile.Models;
using DeliveryProductMobile.Services;
using DeliveryProductMobile.Sessions;
using DeliveryProductMobile.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DeliveryProductMobile.API
{
    class UserApi
    {
        private readonly HttpClient _client;

        public UserApi()
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
            _client = new HttpClient(handler);
        }

        public async Task<string> SignIn(string login, string password)
        {
            try
            {
                HttpResponseMessage response;
                if (!Config.IsEmail(login))
                {
                    response = await _client.PostAsJsonAsync($"{Config.API_HOST}/api/user/signin", new
                    {
                        Login = login,
                        Password = password
                    });
                }
                else
                {
                    response = await _client.PostAsJsonAsync($"{Config.API_HOST}/api/user/signin", new
                    {
                        EmailAddress = login,
                        Password = password
                    });
                }

                if (!response.IsSuccessStatusCode)
                    return await response.Content.ReadAsStringAsync();
                string token = JsonSerializer.Deserialize<TokenModel>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                })?.Token ?? string.Empty;
                if (string.IsNullOrEmpty(token))
                    return "Ошибка сервера #2";

                await LocalStorage.SaveTokenAsync(token);
                await Auth();
                return "SUCCESS";
            }
            catch (Exception e)
            {
                    return "Ошибка серверa #1";
            }
        }

        public async Task<string> SignUp(string login, string emailAddress, string password)
        {
            try
            {
                var response = await _client.PostAsJsonAsync($"{Config.API_HOST}/api/user/signup", new
                {
                    Login = login,
                    Password = password,
                    EmailAddress = emailAddress
                });
                if (!response.IsSuccessStatusCode)
                    return await response.Content.ReadAsStringAsync();

                string token = JsonSerializer.Deserialize<TokenModel>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                })?.Token ?? string.Empty;
                if (string.IsNullOrEmpty(token))
                    return "Ошибка сервера #2";

                await LocalStorage.SaveTokenAsync(token);
                await Auth();
                return "SUCCESS";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public async Task Auth()
        {
            try
            {
                _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", await LocalStorage.LoadTokenAsync());
                var response = await _client.GetAsync($"{Config.API_HOST}/api/user/auth");
                if (!response.IsSuccessStatusCode)
                {
                    LocalStorage.ClearToken();
                }
                var user = JsonSerializer.Deserialize<UserModel>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                if (user == null)
                    return;

                UserSession.User = user;
                if (Application.Current?.ApplicationLifetime is ISingleViewApplicationLifetime app)
                {
                    app.MainView = new MainView();
                }
            }
            catch (Exception e)
            {
            }
        }

        public async Task<List<OrderModel>> GetMyOrders(bool onlyActives = true)
        {
            var step = 0;
            try
            {
                step = 1;
                _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", await LocalStorage.LoadTokenAsync());
                step = 2;
                HttpResponseMessage response = onlyActives ? await _client.GetAsync($"{Config.API_HOST}/api/user/my_active_orders") : await _client.GetAsync($"{Config.API_HOST}/api/user/my_completed_orders");
                step = 3;
                if (!response.IsSuccessStatusCode)
                    return new List<OrderModel>()
                    {
                        new OrderModel()
                        {
                            Id = 1,
                            IsCompleted = false,
                            Status = $"error: {response.StatusCode}",
                            StatusId = 1,
                            TotalPrice = 500,
                            Products = new ()
                        }
                    };
                step = 4;

                return Newtonsoft.Json.JsonConvert.DeserializeObject<List<OrderModel>>(await response.Content.ReadAsStringAsync()) ?? new ();

            }
            catch (Exception e)
            {
                return new List<OrderModel>()
                    {
                        new OrderModel()
                        {
                            Id = step,
                            IsCompleted = false,
                            Status = $"{e.Message}",
                            StatusId = 1,
                            TotalPrice = 500,
                            Products = new ()
                        }
                    };
            }
        }

        public async Task<string> ChangeLogin(string login)
        {
            try
            {
                _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", await LocalStorage.LoadTokenAsync());
                var response = await _client.PatchAsync($"{Config.API_HOST}/api/user/change_login/{login}", null);
                if (response.IsSuccessStatusCode)
                {
                    await Auth();
                    return "SUCCESS";
                }
                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex) { }
            return "Ошибка сервера";
        }

        public async Task<string> ChangeEmail(string email)
        {
            try
            {
                _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", await LocalStorage.LoadTokenAsync());
                var response = await _client.PatchAsync($"{Config.API_HOST}/api/user/change_email/{email}", null);
                if (response.IsSuccessStatusCode)
                {
                    await Auth();
                    return "SUCCESS";
                }
                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex) { }
            return "Ошибка сервера";
        }

        public async Task<string> ChangePassword(string password)
        {
            try
            {
                _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", await LocalStorage.LoadTokenAsync());
                var response = await _client.PatchAsync($"{Config.API_HOST}/api/user/change_password/{password}", null);
                if (response.IsSuccessStatusCode)
                {
                    await Auth();
                    return "SUCCESS";
                }
                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex) { }
            return "Ошибка сервера";
        }

        public async Task<bool> Delete()
        {
            try
            {
                _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", await LocalStorage.LoadTokenAsync());
                var response = await _client.DeleteAsync($"{Config.API_HOST}/api/user/delete");

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex) { }
            return false;
        }
    }
}
