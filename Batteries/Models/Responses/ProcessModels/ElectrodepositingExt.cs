using Batteries.Models.ProcessModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models.Responses.ProcessModels
{
    public class ElectrodepositingExt : Electrodepositing
    {
        public string equipmentName { get; set; }

        public ElectrodepositingExt(Electrodepositing e)
        {
            if (e != null)
            {
                this.electrodepositingId = e.electrodepositingId;
                this.fkExperimentProcess = e.fkExperimentProcess;
                this.fkBatchProcess = e.fkBatchProcess;
                this.fkEquipment = e.fkEquipment;
                this.currentDensity = e.currentDensity;
                this.voltage = e.voltage;
                this.time = e.time;
                this.comments = e.comments;
                this.label = e.label;
                this.dateCreated = e.dateCreated;

            }
        }
    }
}