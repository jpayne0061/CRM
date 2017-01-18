using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CRM.ViewModels
{
    public class FutureDate : ValidationAttribute
    {
        public override bool IsValid(object value)
        {

            DateTime dt = (DateTime)value;

            //why doesn't this work?
            //if(value is DateTime)
            //{
            //    dt = (DateTime)value;
            //}

            return (value is DateTime && dt > DateTime.Now);
        }

    }
}