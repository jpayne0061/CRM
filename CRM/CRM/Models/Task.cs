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

        public bool IsComplete { get; set; }

        public TimeLeft Due()
        {
            DateTime timeNow = DateTime.Now;

            if (Deadline - timeNow < new TimeSpan(24, 0, 0) && Deadline - timeNow > new TimeSpan(24, 0, 0))
            {
                return TimeLeft.LessThan24;
            }
            else if(Deadline - timeNow < new TimeSpan(-24, 0, 0))
            {
                return TimeLeft.OverDue;
            }
            else
            {
                return TimeLeft.NoDanger;
            }


        }

        public string DueStyle()
        {
            if (this.Due() == TimeLeft.OverDue)
            {
                return "over-due";
            }

            if (this.Due() == TimeLeft.LessThan24)
            {
                return "less-than-24";
            }
            if (this.Due() == TimeLeft.NoDanger)
            {
                return "no-danger";
            }
            else
            {
                return "";
            }
        }

        public int DaysLate() {
            if((int)((Deadline - DateTime.Now).TotalDays) > 0)
            {
                return 0;
            }
            else
            {
                return Math.Abs((int)((Deadline - DateTime.Now).TotalDays));
            }
            

        }

        public int HoursLate()
        {
            return (int)((Math.Abs(((Deadline - DateTime.Now).TotalDays) % 1)) * 24);


        }


    }
}