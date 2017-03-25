using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRM.Models
{
    public class CustomerFile
    {
        public int Id { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public Customer Customer { get; set; }
        public ApplicationUser UploadedBy { get; set; } 
        public DateTime DateTime { get; set; }


    }
}