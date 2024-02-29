using Batteries.Models.ProcessModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models.Responses.ProcessModels
{
    public class ElectroplatingExt : Electroplating
    {
        public string equipmentName { get; set; }

        public ElectroplatingExt(Electroplating e)
        {
            if (e != null)
            {
                this.electroplatingId = e.electroplatingId;
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