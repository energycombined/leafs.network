using Batteries.Models.ProcessModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models.Responses.ProcessModels
{
    public class DryingExt : Drying
    {
        public string equipmentName { get; set; }

        public DryingExt(Drying e)
        {
            if (e != null)
            {
                this.dryingId = e.dryingId;
                this.fkExperimentProcess = e.fkExperimentProcess;
                this.fkBatchProcess = e.fkBatchProcess;
                this.fkEquipment = e.fkEquipment;
                this.atmosphere = e.atmosphere;
                this.gasFlow = e.gasFlow;
                this.temperature = e.temperature;
                this.time = e.time;
                this.comments = e.comments;
                this.label = e.label;
                this.dateCreated = e.dateCreated;

            }
        }
    }
}