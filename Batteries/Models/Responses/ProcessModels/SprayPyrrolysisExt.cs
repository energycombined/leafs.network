using Batteries.Models.ProcessModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models.Responses.ProcessModels
{
    public class SprayPyrrolysisExt : SprayPyrrolysis
    {
        public string equipmentName { get; set; }

        public SprayPyrrolysisExt(SprayPyrrolysis e)
        {
            if (e != null)
            {
                this.sprayPyrrolysisId = e.sprayPyrrolysisId;
                this.fkExperimentProcess = e.fkExperimentProcess;
                this.fkBatchProcess = e.fkBatchProcess;
                this.fkEquipment = e.fkEquipment;
                this.temperature = e.temperature;
                this.time = e.time;
                this.comments = e.comments;
                this.label = e.label;
                this.dateCreated = e.dateCreated;
            }
        }
    }
}