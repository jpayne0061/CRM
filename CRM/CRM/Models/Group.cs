using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRM.Models
{
    public class Group
    {

        public Group()
        {
            Users = new List<ApplicationUser>();
            Customers = new List<Customer>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<ApplicationUser> Users { get; set; }
        public ICollection<Customer> Customers { get; set; }
        public ApplicationUser Manager { get; set; }
        public string ManagerId { get; set; }


    }
}