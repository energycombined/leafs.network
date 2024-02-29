using Batteries.Models.ProcessModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models.Responses.ProcessModels
{
    public class PastingExt : Pasting
    {
        public string equipmentName { get; set; }

        public PastingExt(Pasting e)
        {
            if (e != null)
            {
                this.pastingId = e.pastingId;
                this.fkExperimentProcess = e.fkExperimentProcess;
                this.fkBatchProcess = e.fkBatchProcess;
                this.fkEquipment = e.fkEquipment;
                this.thickness = e.thickness;
                this.rollSpeed = e.rollSpeed;
                this.temperature = e.temperature;
                this.substrate = e.substrate;
                this.time = e.time;
                this.comments = e.comments;
                this.label = e.label;
                this.dateCreated = e.dateCreated;

            }
        }
    }
}