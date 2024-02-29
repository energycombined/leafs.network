using Batteries.Models.ProcessModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models.Responses.ProcessModels
{
    public class CoolingExt : Cooling
    {
        public string equipmentName { get; set; }

        public CoolingExt(Cooling e)
        {
            if (e != null)
            {
                this.coolingId = e.coolingId;
                this.fkExperimentProcess = e.fkExperimentProcess;
                this.fkBatchProcess = e.fkBatchProcess;
                this.fkEquipment = e.fkEquipment;
                this.temperature = e.temperature;
                this.time = e.time;
                this.vacuum = e.vacuum;
                this.comments = e.comments;
                this.label = e.label;
                this.dateCreated = e.dateCreated;

            }
        }
    }
}