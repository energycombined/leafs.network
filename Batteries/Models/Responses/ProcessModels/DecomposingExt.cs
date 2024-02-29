using Batteries.Models.ProcessModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models.Responses.ProcessModels
{
    public class DecomposingExt : Decomposing
    {
        public string equipmentName { get; set; }

        public DecomposingExt(Decomposing e)
        {
            if (e != null)
            {
                this.decomposingId = e.decomposingId;
                this.fkExperimentProcess = e.fkExperimentProcess;
                this.fkBatchProcess = e.fkBatchProcess;
                this.fkEquipment = e.fkEquipment;
                this.temperature = e.temperature;
                this.comments = e.comments;
                this.label = e.label;
                this.dateCreated = e.dateCreated;

            }
        }
    }
}