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

        public TimeLeft Due()
        {
            DateTime timeNow = DateTime.Now;

            if (Deadline - timeNow < new TimeSpan(24, 0, 0) && Deadline - timeNow > new TimeSpan(24, 0, 0))
            {
                return TimeLeft.LessThan24;
            }
            else if(Deadline - timeNow < new TimeSpan(0, 0, 0))
            {
                return TimeLeft.OverDue;
            }
            else
            {
                return TimeLeft.NoDanger;
            }


        }


    }
}