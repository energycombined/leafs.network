using Batteries.Models.ProcessModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models.Responses.ProcessModels
{
    public class CentrifugingExt : Centrifuging
    {
        public string equipmentName { get; set; }

        public CentrifugingExt(Centrifuging e)
        {
            if (e != null)
            {
                this.centrifugingId = e.centrifugingId;
                this.fkExperimentProcess = e.fkExperimentProcess;
                this.fkBatchProcess = e.fkBatchProcess;
                this.fkEquipment = e.fkEquipment;
                this.speed = e.speed;
                this.cupSize = e.cupSize;
                this.time = e.time;
                this.comments = e.comments;
                this.label = e.label;
                this.dateCreated = e.dateCreated;

            }
        }
    }
}