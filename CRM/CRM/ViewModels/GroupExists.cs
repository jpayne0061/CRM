using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using CRM.Models;

namespace CRM.ViewModels
{
    public class GroupExists : ValidationAttribute
    {

        private readonly ApplicationDbContext _context;

        public GroupExists()
        {
            _context = new ApplicationDbContext();
        }

        public override bool IsValid(object value)
        {
            string groupName = (string)value;

            var groupLength = _context.Groups.Where(g => g.Name == groupName).ToList().Count();

            return (groupLength < 1);
        }


    }
}