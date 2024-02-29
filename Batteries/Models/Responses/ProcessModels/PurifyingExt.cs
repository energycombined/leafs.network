using Batteries.Models.ProcessModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models.Responses.ProcessModels
{
    public class PurifyingExt : Purifying
    {
        public string equipmentName { get; set; }

        public PurifyingExt(Purifying e)
        {
            if (e != null)
            {
                this.purifyingId = e.purifyingId;
                this.fkExperimentProcess = e.fkExperimentProcess;
                this.fkBatchProcess = e.fkBatchProcess;
                this.fkEquipment = e.fkEquipment;
                this.pressure = e.pressure;
                this.temperature = e.temperature;
                this.time = e.time;
                this.comments = e.comments;
                this.label = e.label;
                this.dateCreated = e.dateCreated;

            }
        }
    }
}