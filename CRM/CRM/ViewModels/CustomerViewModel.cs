﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CRM.Models;
namespace CRM.ViewModels
{
    public class CustomerViewModel
    {


        public string Name { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public int TransferId { get; set; }


        [Display(Name = "Select Team")]
        public List<string> SelectedUsers { get; set; }

        public List<SelectListItem> Users { get; set; }

        public int EditId { get; set; }

        //new checkboxes try
        public List<UserCheckBox> UserCheckBoxes { get; set; }



    }
}