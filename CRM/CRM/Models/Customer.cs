using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CRM.Models
{
    public class Customer
    {

        public Customer()
        {
            Messages = new List<Message>();
            Tasks = new List<Task>();
            Team = new List<ApplicationUser>();
        }

        public ICollection<Message> Messages { get; set; }
        public ICollection<Task> Tasks { get; set; }
        public ICollection<ApplicationUser> Team { get; set; }
        public Group Group { get; set; }


        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [Required]
        [StringLength(255)]
        public string Phone { get; set; }

        [Required]
        [StringLength(255)]
        public string Email { get; set; }

        public int OverDueTasks()
        {
            return Tasks.Where(t => t.Due() == TimeLeft.OverDue).ToList().Count();
        }


    }
}