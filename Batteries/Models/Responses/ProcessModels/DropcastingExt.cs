using Batteries.Models.ProcessModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models.Responses.ProcessModels
{
    public class DropcastingExt : Dropcasting
    {
        public string equipmentName { get; set; }

        public DropcastingExt(Dropcasting e)
        {
            if (e != null)
            {
                this.dropcastingId = e.dropcastingId;
                this.fkExperimentProcess = e.fkExperimentProcess;
                this.fkBatchProcess = e.fkBatchProcess;
                this.fkEquipment = e.fkEquipment;
                this.volume = e.volume;
                this.concentration = e.concentration;
                this.time = e.time;
                this.comments = e.comments;
                this.label = e.label;
                this.dateCreated = e.dateCreated;

            }
        }
    }
}