using Batteries.Models.ProcessModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models.Responses.ProcessModels
{
    public class ScreenprintingExt : Screenprinting
    {
        public string equipmentName { get; set; }

        public ScreenprintingExt(Screenprinting e)
        {
            if (e != null)
            {
                this.screenprintingId = e.screenprintingId;
                this.fkExperimentProcess = e.fkExperimentProcess;
                this.fkBatchProcess = e.fkBatchProcess;
                this.fkEquipment = e.fkEquipment;
                this.screenMeshSize = e.screenMeshSize;
                this.thickness = e.thickness;
                this.time = e.time;
                this.comments = e.comments;
                this.label = e.label;
                this.dateCreated = e.dateCreated;

            }
        }
    }
}