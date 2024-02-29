using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models.Responses.ProcessModels
{
    public class ProcessRequest
    {
        public dynamic processAttributes { get; set; }
        public int? step { get; set; }
        public int? processTypeId { get; set; }
        public int? equipmentId { get; set; }
        public int? equipmentModelId { get; set; }
        public int? processOrderInStep { get; set; }
        public string processLabel { get; set; }
        
    }
}