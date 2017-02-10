using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Models;


namespace CRM.Models
{
    public class UserNotification
    {
        public int Id { get; set; }
        public string Sender { get; set; }
        public string CustomerName { get; set; }
        public string Body { get; set; }
        public ApplicationUser Recipient { get; set; }
        public string RecipientId { get; set; }
        public bool IsRead { get; set; }
        public int CustomerId { get; set; }

    }
}