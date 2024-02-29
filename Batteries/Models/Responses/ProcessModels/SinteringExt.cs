using Batteries.Models.ProcessModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models.Responses.ProcessModels
{
    public class SinteringExt : Sintering
    {
        public string equipmentName { get; set; }

        public SinteringExt(Sintering e)
        {
            if (e != null)
            {
                this.sinteringId = e.sinteringId;
                this.fkExperimentProcess = e.fkExperimentProcess;
                this.fkBatchProcess = e.fkBatchProcess;
                this.fkEquipment = e.fkEquipment;
                this.temperature = e.temperature;
                this.time = e.time;
                this.rampUpTime = e.rampUpTime;
                this.rampDownTime = e.rampDownTime;
                this.plateauTime = e.plateauTime;
                this.atmosphere = e.atmosphere;
                this.comments = e.comments;
                this.label = e.label;
                this.dateCreated = e.dateCreated;

            }
        }
    }
}