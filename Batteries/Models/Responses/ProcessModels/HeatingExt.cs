using Batteries.Models.ProcessModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models.Responses.ProcessModels
{
    public class HeatingExt : Heating
    {
        public string equipmentName { get; set; }
        public HeatingExt(Heating e)
        {
            this.heatingId = e.heatingId;
            this.fkExperimentProcess = e.fkExperimentProcess;
            this.fkBatchProcess = e.fkBatchProcess;
            this.fkEquipment = e.fkEquipment;
            this.comments = e.comments;
            this.label = e.label;
            this.dateCreated = e.dateCreated;
        }
    }
}