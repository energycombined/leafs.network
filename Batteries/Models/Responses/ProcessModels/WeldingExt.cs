using Batteries.Models.ProcessModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models.Responses.ProcessModels
{
    public class WeldingExt : Welding
    {
        public string equipmentName { get; set; }

        public WeldingExt(Welding e)
        {
            if (e != null)
            {
                this.weldingId = e.weldingId;
                this.fkExperimentProcess = e.fkExperimentProcess;
                this.fkBatchProcess = e.fkBatchProcess;
                this.fkEquipment = e.fkEquipment;
                this.weldingPointsNumber = e.weldingPointsNumber;
                this.time = e.time;
                this.comments = e.comments;
                this.label = e.label;
                this.dateCreated = e.dateCreated;

            }
        }
    }
}