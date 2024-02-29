using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models.ProcessModels
{
    public class Centrifuging
    {
        public long centrifugingId { get; set; }
        public long? fkExperimentProcess { get; set; }
        public long? fkBatchProcess { get; set; }
        public int? fkEquipment { get; set; }
        public double? speed { get; set; }
        public double? cupSize { get; set; }
        public double? time { get; set; }
        public string comments { get; set; }
        public string label { get; set; }
        public DateTime? dateCreated { get; set; }

    }
}