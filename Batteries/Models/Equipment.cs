using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models
{
    public class Equipment
    {
        public int equipmentId { get; set; }
        public string equipmentName { get; set; }
        public string equipmentLabel { get; set; }
        public int? fkProcessType { get; set; }

    }
}