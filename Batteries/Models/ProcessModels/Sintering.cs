using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models.ProcessModels
{
    public class Sintering
    {
        public long sinteringId { get; set; }
        public long? fkExperimentProcess { get; set; }
        public long? fkBatchProcess { get; set; }
        public int? fkEquipment { get; set; }
        public double? temperature { get; set; }
        public double? time { get; set; }
        public double? rampUpTime { get; set; }
        public double? rampDownTime { get; set; }
        public double? plateauTime { get; set; }
        public string atmosphere { get; set; }
        public string comments { get; set; }
        public string label { get; set; }
        public DateTime? dateCreated { get; set; }

    }
}