using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models.ProcessModels
{
    public class Sieving
    {
        public long sievingId { get; set; }
        public long? fkExperimentProcess { get; set; }
        public long? fkBatchProcess { get; set; }
        public int? fkEquipment { get; set; }
        public double? sieveWidth { get; set; }
        public string sieveMaterial { get; set; }
        public double? time { get; set; }
        public string comments { get; set; }
        public string label { get; set; }
        public DateTime? dateCreated { get; set; }

    }
}