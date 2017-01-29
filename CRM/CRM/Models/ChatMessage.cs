using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Models;

namespace CRM.Models
{
    public class ChatMessage
    {

        public int Id { get; set; }
        public ApplicationUser Sender { get; set; }
        public string SenderId { get; set; }
        public ApplicationUser Receiver { get; set; }
        public string ReceiverId { get; set; }
        public string Body { get; set; }
        public DateTime TimeStamp { get; set; }
        public bool IsRead { get; set; }

    }
}