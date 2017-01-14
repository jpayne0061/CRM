using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CRM.Models
{
    public class UserMessage
    {

        [Key]
        [Column(Order = 1)]
        public string UserId { get; set; }

        [Key]
        [Column(Order = 2)]
        public int MessageId { get; set; }
        

        public Message Message { get; set; }
        public ApplicationUser User { get; set; }

    }
}