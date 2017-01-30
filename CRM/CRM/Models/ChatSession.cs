using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Models;


namespace CRM.Models
{
    public class ChatSession
    {
        public int Id { get; set; }
        public ICollection<ChatMessage> ChatMessages { get; set; }
        public bool IsActive { get; set; }
        public string ReceiverId { get; set; }
        public string SenderId { get; set; }
        public string RequestingUser { get; set; }

        public ChatSession()
        {
            ChatMessages = new List<ChatMessage>();
            IsActive = true;
        }




    }
}