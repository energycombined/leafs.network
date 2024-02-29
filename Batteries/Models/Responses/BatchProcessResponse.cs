using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models.Responses
{
    public class BatchProcessResponse
    {
        public BatchProcessExt batchProcess { get; set; }
        public dynamic processAttributes { get; set; }
        public dynamic equipmentSettings { get; set; }
        public int experimentId { get; set; }
        public string batteryComponentType { get; set; }
    }
}