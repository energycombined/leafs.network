using Batteries.Models.ProcessModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models.Responses.ProcessModels
{
    public class ElectrodeWindingExt : ElectrodeWinding
    {
        public string equipmentName { get; set; }

        public ElectrodeWindingExt(ElectrodeWinding e)
        {
            if (e != null)
            {
                this.electrodeWindingId = e.electrodeWindingId;
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