using Batteries.Models.ProcessModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models.Responses.ProcessModels
{
    public class WetImpregnatingExt : WetImpregnating
    {
        public string equipmentName { get; set; }

        public WetImpregnatingExt(WetImpregnating e)
        {
            if (e != null)
            {
                this.wetImpregnatingId = e.wetImpregnatingId;
                this.fkExperimentProcess = e.fkExperimentProcess;
                this.fkBatchProcess = e.fkBatchProcess;
                this.fkEquipment = e.fkEquipment;
                this.solutionType = e.solutionType;
                this.concentration = e.concentration;
                this.volume = e.volume;
                this.time = e.time;
                this.comments = e.comments;
                this.label = e.label;
                this.dateCreated = e.dateCreated;

            }
        }
    }
}