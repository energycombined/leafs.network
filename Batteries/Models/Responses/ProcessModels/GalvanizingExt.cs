using Batteries.Models.ProcessModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models.Responses.ProcessModels
{
    public class GalvanizingExt : Galvanizing
    {
        public string equipmentName { get; set; }

        public GalvanizingExt(Galvanizing e)
        {
            if (e != null)
            {
                this.galvanizingId = e.galvanizingId;
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