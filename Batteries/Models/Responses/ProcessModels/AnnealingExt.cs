using Batteries.Models.ProcessModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models.Responses.ProcessModels
{
    public class AnnealingExt : Annealing
    {
        public string equipmentName { get; set; }

        public AnnealingExt(Annealing e)
        {
            if (e != null)
            {
                this.annealingId = e.annealingId;
                this.fkExperimentProcess = e.fkExperimentProcess;
                this.fkBatchProcess = e.fkBatchProcess;
                this.fkEquipment = e.fkEquipment;
                this.rampUpTime = e.rampUpTime;
                this.rampDownTime = e.rampDownTime;
                this.plateauTime = e.plateauTime;
                this.atmosphere = e.atmosphere;
                this.temperature = e.temperature;
                this.time = e.time;
                this.comments = e.comments;
                this.label = e.label;
                this.dateCreated = e.dateCreated;

            }
        }
    }
}