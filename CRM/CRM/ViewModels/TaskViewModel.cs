using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Models;

namespace CRM.ViewModels
{
    public class TaskViewModel
    {

        public string Title { get; set; }
        //public Customer Customer { get; set; }
        public int CustomerId { get; set; }
        public string AssignedTo { get; set; }
        public string AssignedBy { get; set; }
        public DateTime Deadline { get; set; }
        public string Body { get; set; }
        public IEnumerable<ApplicationUser> Users { get; set; }

    }
}