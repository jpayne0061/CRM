using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRM.Models
{
    public class DeleteTeamMember
    {
        public int CustomerId { get; set; }
        public string TeamMemberId { get; set; }
    }
}