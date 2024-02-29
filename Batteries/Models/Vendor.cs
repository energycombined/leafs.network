using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models
{
    public class Vendor
    {
        public int vendorId { get; set; }
        public string vendorName { get; set; }
        public string vendorSite { get; set; }
        public DateTime? lastChange { get; set; }
        public string contactPerson { get; set; }
        public string phoneNumber { get; set; }
        public string comment { get; set; }

    }
}