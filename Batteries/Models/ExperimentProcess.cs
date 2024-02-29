using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Batteries.Models
{
    public class ExperimentProcess
    {
        public long experimentProcessId { get; set; }
        [Required]
        public int? fkExperiment { get; set; }
        public int? fkBatteryComponentType { get; set; }
        [Required]
        public int? step { get; set; }
        //public int? fkProcessType { get; set; }
        [Required]
        public int? processOrderInStep { get; set; }
        public Boolean? isComplete { get; set; }
        public string label { get; set; }
        //public int? fkEquipment { get; set; }
        public DateTime? dateCreated { get; set; }
        public int? fkProcess { get; set; }

    }
}