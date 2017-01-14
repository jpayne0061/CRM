using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Models;
using System.Web.Mvc;

namespace CRM.ViewModels
{
    public class MessageViewModel
    {
        //public int Id { get; set; }
        public string Body { get; set; }
        //public Customer Customer { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public List<string> SelectedEmails { get; set; }
        public List<SelectListItem> Users { get; set; }
        
    }
}