using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Batteries.Models.ProcessModels
{
    public class Milling
    {
        public long millingId { get; set; }
        public long? fkExperimentProcess { get; set; }
        public long? fkBatchProcess { get; set; }
        public int? fkEquipment { get; set; }
        //[Required]
        //public double? ballMillCupsSize { get; set; }
        //public string ballMillCupsMaterial { get; set; }
        //public string ballMillCupsVendor { get; set; }
        //public double? ballsSize { get; set; }
        //public string ballsMaterial { get; set; }
        //public int? ballsAmount { get; set; }
        //public int? millingSpeedRpm { get; set; }
        //public double? restingTime { get; set; }
        //public int? loopCount { get; set; }
        //public double? ballPowderRatio { get; set; }
        public string comments { get; set; }
        public string label { get; set; }
        public DateTime? dateCreated { get; set; }
    }
}