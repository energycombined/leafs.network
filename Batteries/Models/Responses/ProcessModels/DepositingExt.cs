using Batteries.Models.ProcessModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models.Responses.ProcessModels
{
    public class DepositingExt : Depositing
    {
        public string equipmentName { get; set; }

        public DepositingExt(Depositing e)
        {
            if (e != null)
            {
                this.depositingId = e.depositingId;
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