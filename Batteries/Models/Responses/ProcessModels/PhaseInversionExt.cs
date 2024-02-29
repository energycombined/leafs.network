using Batteries.Models.ProcessModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models.Responses.ProcessModels
{
    public class PhaseInversionExt : PhaseInversion
    {
        public string equipmentName { get; set; }
        public PhaseInversionExt(PhaseInversion e)
        {
            if (e != null)
            {
                this.phaseInversionId = e.phaseInversionId;
                this.fkExperimentProcess = e.fkExperimentProcess;
                this.fkBatchProcess = e.fkBatchProcess;
                this.fkEquipment = e.fkEquipment;
                this.coagulationBath = e.coagulationBath;
                this.additives = e.additives;
                this.temperature = e.temperature;
                this.time = e.time;
                this.stirring = e.stirring;
                this.stirringSpeed = e.stirringSpeed;
                this.comments = e.comments;
                this.label = e.label;
                this.dateCreated = e.dateCreated;
            }
        }
    }
}