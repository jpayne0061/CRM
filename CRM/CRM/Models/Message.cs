using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRM.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string Body { get; set; }
        public Customer Customer { get; set; }
        public int CustomerId { get; set; }


    }
}