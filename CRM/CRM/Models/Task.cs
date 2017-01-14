using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRM.Models
{
    public class Task
    {

        public int Id { get; set; }

        public Customer Customer { get; set; }
        public int CustomerId { get; set; }

        public ApplicationUser AssignedTo { get; set; }
        public string AssignedToId { get; set; }

        public ApplicationUser AssignedBy { get; set; }
        public string AssignedById { get; set; }

        public DateTime Deadline { get; set; }
        public string Body { get; set; }


    }
}