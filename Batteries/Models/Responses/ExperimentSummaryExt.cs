using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models.Responses
{
    public class ExperimentSummaryExt : ExperimentSummary
    {
        public string commercialTypeName { get; set; }
        public ExperimentSummaryExt(ExperimentSummary e = null)
        {
            if (e != null)
            {
                this.experimentSummaryId = e.experimentSummaryId;
                this.fkExperiment = e.fkExperiment;
                this.componentEmpty = e.componentEmpty;
                this.totalWeight = e.totalWeight;
                this.totalLabeledMaterials = e.totalLabeledMaterials;
                this.labeledMaterials = e.labeledMaterials;
                this.labeledPercentages = e.labeledPercentages;
                this.totalActiveMaterials = e.totalActiveMaterials;
                this.totalActiveMaterialsPercentage = e.totalActiveMaterialsPercentage;
                this.activeMaterials = e.activeMaterials;
                this.activePercentages = e.activePercentages;
                this.fkBatteryComponentType = e.fkBatteryComponentType;
                this.fkCommercialType = e.fkCommercialType;
                //this.mass1 = e.mass1;
                //this.mass2 = e.mass2;
                //this.mass3 = e.mass3;
                //this.mass4 = e.mass4;
                //this.mass5 = e.mass5;
                //this.mass6 = e.mass6;

            }
        }
    }
}