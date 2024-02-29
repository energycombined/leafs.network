using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models
{
    public class ExperimentSummary
    {
        public long experimentSummaryId { get; set; }
        public int? fkExperiment { get; set; }
        public Boolean? componentEmpty { get; set; }
        public double? totalWeight { get; set; }
        public double? totalLabeledMaterials { get; set; }
        public string labeledMaterials { get; set; }
        public string labeledPercentages { get; set; }
        public double? totalActiveMaterials { get; set; }
        public double? totalActiveMaterialsPercentage { get; set; }
        public string activeMaterials { get; set; }
        public string activePercentages { get; set; }
        public int? fkBatteryComponentType { get; set; }
        public long? fkCommercialType { get; set; }
        //public double? mass1 { get; set; }
        //public double? mass2 { get; set; }
        //public double? mass3 { get; set; }
        //public double? mass4 { get; set; }
        //public double? mass5 { get; set; }
        //public double? mass6 { get; set; }

    }
}