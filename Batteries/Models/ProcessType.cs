using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models
{
    public class ProcessType
    {
        public int processTypeId { get; set; }
        public string processType { get; set; }
        public string processDatabaseType { get; set; }
        public string subcategory { get; set; }
    }
}