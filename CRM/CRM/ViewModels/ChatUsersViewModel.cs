using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Models;

namespace CRM.ViewModels
{
    public class ChatUsersViewModel
    {
        public List<ApplicationUser> UsersAvailable { get; set; }
        public List<ApplicationUser> UsersNotAvailable { get; set; }
        public ApplicationUser CurrentUser { get; set; }
    }
}