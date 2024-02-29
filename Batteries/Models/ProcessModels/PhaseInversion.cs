using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models.ProcessModels
{
    public class PhaseInversion
    {
        public long phaseInversionId { get; set; }
        public long? fkExperimentProcess { get; set; }
        public long? fkBatchProcess { get; set; }
        public int? fkEquipment { get; set; }
        public string coagulationBath { get; set; }
        public string additives { get; set; }
        public double? temperature { get; set; }
        public double? time { get; set; }
        public Boolean? stirring { get; set; }
        public double? stirringSpeed { get; set; }
        public string comments { get; set; }
        public string label { get; set; }
        public DateTime? dateCreated { get; set; }

    }
}