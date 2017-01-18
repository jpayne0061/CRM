using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Models;

namespace CRM.ViewModels
{
    public class CustomerDetailViewModel
    {
        public List<Message> Messages { get; set; }
        public List<ApplicationUser> Team {get; set; }
        public List<Task> Tasks { get; set; }
        public int Id { get; set; }
        public string CurrentUserId { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

    }
}