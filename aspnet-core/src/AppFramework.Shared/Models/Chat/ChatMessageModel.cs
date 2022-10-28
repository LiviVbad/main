using AppFramework.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppFramework.Shared.Models.Chat
{
    public class ChatMessageModel
    {
        public long UserId { get; set; }

        public string UserName { get; set; }

        public int? TenantId { get; set; }

        public long TargetUserId { get; set; }

        public int? TargetTenantId { get; set; }

        public ChatSide Side { get; set; }

        public ChatMessageReadState ReadState { get; set; }

        public ChatMessageReadState ReceiverReadState { get; set; }

        public string Message { get; set; }

        public DateTime CreationTime { get; set; }

        public string SharedMessageId { get; set; } 
    }
}
