using Avalonia.Controls;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using DeliveryProductMobile.API;
using DeliveryProductMobile.Models;
using DeliveryProductMobile.Services;
using DeliveryProductMobile.Sessions;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryProductMobile.ViewModels
{
    public partial class ChatViewModel : ViewModelBase
    {
        HubConnection? _hubConnection;
        ChatApi _chatApi = new();

        public int Val = 1;

        public ScrollViewer? ScrollViewerMessages;

        [ObservableProperty]
        private int chatId;

        [ObservableProperty]
        private ObservableCollection<MessageModel> messages = new();

        public ChatViewModel(ScrollViewer scrollViewer)
        {
            ScrollViewerMessages = scrollViewer;
        }

        public async Task Load(int orderId)
        {
            try
            {
                ChatId = await _chatApi.GetId(orderId);
                if (ChatId == 0)
                {
                    if (!await _chatApi.Add(orderId))
                    {
                        NavigationService.GoBack();
                        return;
                    }
                    ChatId = await _chatApi.GetId(orderId);
                    if (ChatId == 0)
                    {
                        NavigationService.GoBack();
                        return;
                    }
                        
                }

                var jwtToken = await LocalStorage.LoadTokenAsync();

                _hubConnection = new HubConnectionBuilder()
               .WithUrl($"{Config.API_HOST}/hubs/chat?access_token=" + await LocalStorage.LoadTokenAsync(), options =>
               {
                   options.HttpMessageHandlerFactory = _ => new HttpClientHandler() { ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true };
                   options.AccessTokenProvider = () => Task.FromResult(jwtToken);
               })
               .WithAutomaticReconnect()
               .Build();

                _hubConnection.On<string>("ReceiveMessage", (jsonMessage) =>
                {
                    Avalonia.Threading.Dispatcher.UIThread.Post(async () =>
                    {
                        var message = Newtonsoft.Json.JsonConvert.DeserializeObject<MessageModel>(jsonMessage) ?? new();
                        if (message.AttachmentPath?.Length > 5)
                            message.Attachment = await ImageLoader.LoadImageFromUrl($"{Config.API_HOST}{message.AttachmentPath}");

                        Messages.Add(message);
                        ScrollViewerMessages?.ScrollToEnd();
                    });
                });

                _hubConnection.On<string>("ReceiveImage", (jsonMessage) =>
                {
                    Avalonia.Threading.Dispatcher.UIThread.Post(async () =>
                    {
                        var message = Newtonsoft.Json.JsonConvert.DeserializeObject<MessageModel>(jsonMessage) ?? new();
                        if (message.AttachmentPath?.Length > 5)
                        {
                            message.Attachment = await ImageLoader.LoadImageFromUrl($"{Config.API_HOST}{message.AttachmentPath}");
                            Messages.Add(message);
                        }
                        ScrollViewerMessages?.ScrollToEnd();
                    });
                });

                _hubConnection.On<string>("ReceiveHistory", (jsonMessage) =>
                {
                    Avalonia.Threading.Dispatcher.UIThread.Post(async () =>
                    {
                        var m = Newtonsoft.Json.JsonConvert.DeserializeObject<List<MessageModel>>(jsonMessage) ?? new();
                        Messages.Clear();
                        foreach (var item in m)
                        {
                            if (item.AttachmentPath.Length > 5)
                                item.Attachment = await ImageLoader.LoadImageFromUrl($"{Config.API_HOST}{item.AttachmentPath}");
                            Messages.Add(item);
                        }
                        ScrollViewerMessages?.ScrollToEnd();
                    });
                    


                });



                await _hubConnection.StartAsync();
                await _hubConnection.InvokeAsync("JoinChat", ChatId);

                Messages.Add(new()
                {
                    Id = 11,
                    AttachmentPath = "",
                    Content = $"COntent: {_hubConnection.ConnectionId}",
                    CreatedAt = DateTime.Now,
                    SenderId = 1,
                    SenderLogin = "f"
                });

            }
            catch (Exception ex) { }
           
        }

        public async Task SendMessage(string content)
        {
            if (_hubConnection == null)
                return;

            await _hubConnection.InvokeAsync("SendMessage", ChatId, content);
        }

        public async Task SendImage(IStorageFile file)
        {

            if (_hubConnection == null) return;
            
            string path = await _chatApi.UploadImage(ChatId, file);

            if (path.Length < 5)
                return;
            await _hubConnection.InvokeAsync("SendImage", ChatId, path);
        }
    }
}
