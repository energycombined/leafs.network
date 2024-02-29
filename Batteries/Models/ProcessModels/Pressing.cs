using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models.ProcessModels
{
    public class Pressing
    {
        public long pressingId { get; set; }
        public long? fkExperimentProcess { get; set; }
        public long? fkBatchProcess { get; set; }
        public int? fkEquipment { get; set; }
        public string comments { get; set; }
        public string label { get; set; }
        public DateTime? dateCreated { get; set; }
        //public string pressingBlocks { get; set; }
        //public string substrate { get; set; }
        //public string material { get; set; }
        //public string vendor { get; set; }
        //public double? pressure { get; set; }
        //public double? pressingTime { get; set; }
        //public double? temperature { get; set; }
    }
}