using Batteries.Models.ProcessModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models.Responses.ProcessModels
{
    public class FormationExt : Formation
    {
        public string equipmentName { get; set; }

        public FormationExt(Formation e)
        {
            if (e != null)
            {
                this.formationId = e.formationId;
                this.fkExperimentProcess = e.fkExperimentProcess;
                this.fkBatchProcess = e.fkBatchProcess;
                this.fkEquipment = e.fkEquipment;
                this.current = e.current;
                this.voltage = e.voltage;
                this.numberOfCycles = e.numberOfCycles;
                this.chargeCapacity = e.chargeCapacity;
                this.dod = e.dod;
                this.time = e.time;
                this.comments = e.comments;
                this.label = e.label;
                this.dateCreated = e.dateCreated;

            }
        }
    }
}