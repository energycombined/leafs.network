using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models.ProcessModels
{
    public class Coating
    {
        public long coatingId { get; set; }
        public long? fkExperimentProcess { get; set; }
        public long? fkBatchProcess { get; set; }
        public int? fkEquipment { get; set; }
        public double? thickness { get; set; }
        public double? width { get; set; }
        public double? length { get; set; }
        public double? dropVolume { get; set; }
        public double? acceleration { get; set; }
        public double? time { get; set; }
        public string comments { get; set; }
        public string label { get; set; }
        public DateTime? dateCreated { get; set; }

    }
}