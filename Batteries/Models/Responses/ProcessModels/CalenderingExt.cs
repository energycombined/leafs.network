using Batteries.Models.ProcessModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models.Responses.ProcessModels
{
    public class CalenderingExt : Calendering
    {
        public string equipmentName { get; set; }
        public CalenderingExt(Calendering e)
        {
            if (e != null)
            {
                this.calenderingId = e.calenderingId;
                this.fkExperimentProcess = e.fkExperimentProcess;
                this.fkBatchProcess = e.fkBatchProcess;
                this.fkEquipment = e.fkEquipment;
                this.comments = e.comments;
                this.label = e.label;
                this.dateCreated = e.dateCreated;
                
                //this.fkExperimentProcess = e.fkExperimentProcess;
                //this.fkBatchProcess = e.fkBatchProcess;
                //this.fkEquipment = e.fkEquipment;
                //this.substrate = e.substrate;
                //this.material = e.material;
                //this.materialType = e.materialType;
                //this.thickness = e.thickness;
                //this.temperature = e.temperature;
                //this.pressure = e.pressure;
                //this.speed = e.speed;
                //this.dateCreated = e.dateCreated;
            }
        }
    }
}