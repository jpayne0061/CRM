using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRM.Models
{
    public class CustomerUploadVM
    {
        public HttpPostedFileBase File { get; set; }
        public int CustomerId { get; set; }

    }
}