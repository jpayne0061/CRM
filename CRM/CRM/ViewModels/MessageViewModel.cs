using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Models;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace CRM.ViewModels
{
    public class MessageViewModel
    {
        public int EditId { get; set; }

        [Required]
        [Display(Name = " ")]
        public string Body { get; set; }
        //public Customer Customer { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }


        [Required]
        [Display(Name="Select Recipients")]
        public List<string> SelectedEmails { get; set; }
        public List<SelectListItem> Users { get; set; }
        
    }
}