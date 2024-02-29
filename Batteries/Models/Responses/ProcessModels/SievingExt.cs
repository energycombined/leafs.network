using Batteries.Models.ProcessModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models.Responses.ProcessModels
{
    public class SievingExt : Sieving
    {
        public string equipmentName { get; set; }

        public SievingExt(Sieving e)
        {
            if (e != null)
            {
                this.sievingId = e.sievingId;
                this.fkExperimentProcess = e.fkExperimentProcess;
                this.fkBatchProcess = e.fkBatchProcess;
                this.fkEquipment = e.fkEquipment;
                this.sieveWidth = e.sieveWidth;
                this.sieveMaterial = e.sieveMaterial;
                this.time = e.time;
                this.comments = e.comments;
                this.label = e.label;
                this.dateCreated = e.dateCreated;

            }
        }
    }
}