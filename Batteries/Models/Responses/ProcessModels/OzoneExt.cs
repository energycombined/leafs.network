using Batteries.Models.ProcessModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models.Responses.ProcessModels
{
    public class OzoneExt : Ozone
    {
        public string equipmentName { get; set; }

        public OzoneExt(Ozone e)
        {
            if (e != null)
            {
                this.ozoneId = e.ozoneId;
                this.fkExperimentProcess = e.fkExperimentProcess;
                this.fkBatchProcess = e.fkBatchProcess;
                this.fkEquipment = e.fkEquipment;
                this.time = e.time;
                this.comments = e.comments;
                this.label = e.label;
                this.dateCreated = e.dateCreated;

            }
        }
    }
}