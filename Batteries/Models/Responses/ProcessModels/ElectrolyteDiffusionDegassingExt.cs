using Batteries.Models.ProcessModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models.Responses.ProcessModels
{
    public class ElectrolyteDiffusionDegassingExt : ElectrolyteDiffusionDegassing
    {
        public string equipmentName { get; set; }

        public ElectrolyteDiffusionDegassingExt(ElectrolyteDiffusionDegassing e)
        {
            if (e != null)
            {
                this.electrolyteDiffusionDegassingId = e.electrolyteDiffusionDegassingId;
                this.fkExperimentProcess = e.fkExperimentProcess;
                this.fkBatchProcess = e.fkBatchProcess;
                this.fkEquipment = e.fkEquipment;
                this.time = e.time;
                this.comments = e.comments;
                this.label = e.label;
                this.dateCreated = e.dateCreated;

            }
        }
    }
}