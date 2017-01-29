using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRM.Models
{
    public enum TimeLeft
    {
        OverDue = 1,
        LessThan24 = 2,
        NoDanger = 3
    }
}