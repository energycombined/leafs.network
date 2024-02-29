using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models
{
    public class MaterialFunction
    {
        public int materialFunctionId { get; set; }
        public string materialFunctionName { get; set; }
        public DateTime? dateCreated { get; set; }
        public DateTime? lastChange { get; set; }

    }
}