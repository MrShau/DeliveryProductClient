using Avalonia.Media.Imaging;
using DeliveryProductMobile.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryProductMobile.Models
{
    public class MessageModel
    {
        public int? Id { get; set; }
        public int? ChatId { get; set; }
        public string? Content { get; set; }
        public int? SenderId { get; set; }
        public string? HorizontalAlignment { get => SenderId != UserSession.User?.Id ? "Left" : "Right"; }
        public string? SenderLogin { get; set; }
        public string? AttachmentPath { get; set; }
        public Bitmap? Attachment { get; set; }
        public int? ImageHeight { get => Attachment == null ? 0 : 252; }
        public DateTime CreatedAt { get; set; }
        public string? CreatedAtString { get => CreatedAt.ToString("dd.MM.yy HH:mm"); }
    }
}
