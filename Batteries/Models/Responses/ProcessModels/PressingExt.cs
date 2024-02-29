using Batteries.Models.ProcessModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models.Responses.ProcessModels
{
    public class PressingExt : Pressing
    {
        public string equipmentName { get; set; }
        public PressingExt(Pressing e)
        {
            if (e != null)
            {
                this.pressingId = e.pressingId;
                this.fkExperimentProcess = e.fkExperimentProcess;
                this.fkBatchProcess = e.fkBatchProcess;
                this.fkEquipment = e.fkEquipment;
                this.comments = e.comments;
                this.label = e.label;
                this.dateCreated = e.dateCreated;

                //this.pressingBlocks = e.pressingBlocks;
                //this.substrate = e.substrate;
                //this.material = e.material;
                //this.vendor = e.vendor;
                //this.pressure = e.pressure;
                //this.pressingTime = e.pressingTime;
                //this.temperature = e.temperature;
            }
        }
    }
}