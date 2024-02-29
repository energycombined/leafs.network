using Batteries.Models.ProcessModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models.Responses.ProcessModels
{
    public class CoatingExt : Coating
    {
        public string equipmentName { get; set; }

        public CoatingExt(Coating e)
        {
            if (e != null)
            {
                this.coatingId = e.coatingId;
                this.fkExperimentProcess = e.fkExperimentProcess;
                this.fkBatchProcess = e.fkBatchProcess;
                this.fkEquipment = e.fkEquipment;
                this.thickness = e.thickness;
                this.width = e.width;
                this.length = e.length;
                this.dropVolume = e.dropVolume;
                this.acceleration = e.acceleration;
                this.time = e.time;
                this.comments = e.comments;
                this.label = e.label;
                this.dateCreated = e.dateCreated;

            }
        }
    }
}