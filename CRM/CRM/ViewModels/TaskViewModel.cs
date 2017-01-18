using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Models;
using System.ComponentModel.DataAnnotations;

namespace CRM.ViewModels
{
    public class TaskViewModel
    {

        //public string Title { get; set; }
        //public Customer Customer { get; set; }
        public int CustomerId { get; set; }

        [Required]
        [Display(Name = "Assign Team Member")]
        public string AssignedTo { get; set; }
        public string AssignedBy { get; set; }

        [FutureDate(ErrorMessage = "We know you want it done yesterday, but the deadline must be in the future")]
        public DateTime Deadline { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string Body { get; set; }

        public IEnumerable<ApplicationUser> Users { get; set; }

        public int EditId { get; set; }

    }
}