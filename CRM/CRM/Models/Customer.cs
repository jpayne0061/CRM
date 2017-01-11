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
        }

        public ICollection<Message> Messages { get; set; }


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


    }
}