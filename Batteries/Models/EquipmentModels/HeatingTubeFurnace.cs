using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models.EquipmentModels
{
    public class HeatingTubeFurnace
    {
        public long settingsId { get; set; }
        public long? fkExperimentProcess { get; set; }
        public long? fkBatchProcess { get; set; }
        public int? fkEquipmentModel { get; set; }

        public string tubeMaterial { get; set; }
        public double? tubeDiameter { get; set; }
        public int? tubeAmountOfOpenings { get; set; }
        public string atmosphere { get; set; }
        public Boolean? flow { get; set; }
        public double? rampUpTime { get; set; }
        public double? temperature { get; set; }
        public double? duration { get; set; }
        public double? rampDownTime { get; set; }
        public int? loopCount { get; set; }
        public string comment { get; set; }
        public string label { get; set; }
        public DateTime? dateCreated { get; set; }

    }
}