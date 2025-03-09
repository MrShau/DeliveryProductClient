using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryProductMobile.Models
{
    public class ChatModel
    {
        public int Id { get; set; }
        public List<MessageModel> Messages { get; set; } = new();
    }
}
