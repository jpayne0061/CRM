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

        //requesting user is user that is calling API get request for current chat sessions
        public string RequestingUser { get; set; }

        //requester name is the user that initiates the chat session
        public string RequesterName { get; set; }

        //user that is target of ceated chat session
        public string RecipientName { get; set; }

        public ChatSession()
        {
            ChatMessages = new List<ChatMessage>();
            IsActive = true;
        }




    }
}