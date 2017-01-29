using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Models;


namespace CRM.Models
{
    public class JoinRequest
    {

        public int Id { get; set; }
        public string RequesterId { get; set; }
        public ApplicationUser Requester { get; set; }
        public string ManagerId { get; set; }

    }
}