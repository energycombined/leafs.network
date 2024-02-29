using Batteries.Models.EquipmentModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models.Responses.EquipmentModels
{
    public class HeatingTubeFurnaceExt : HeatingTubeFurnace
    {
        public string equipmentModelName { get; set; }
        public HeatingTubeFurnaceExt(HeatingTubeFurnace e = null)
        {            

            if (e != null)
            {
                this.settingsId = e.settingsId;
                this.fkExperimentProcess = e.fkExperimentProcess;
                this.fkBatchProcess = e.fkBatchProcess;
                this.fkEquipmentModel = e.fkEquipmentModel;
                this.tubeMaterial = e.tubeMaterial;
                this.tubeDiameter = e.tubeDiameter;
                this.tubeAmountOfOpenings = e.tubeAmountOfOpenings;
                this.atmosphere = e.atmosphere;
                this.flow = e.flow;
                this.rampUpTime = e.rampUpTime;
                this.temperature = e.temperature;
                this.duration = e.duration;
                this.rampDownTime = e.rampDownTime;
                this.loopCount = e.loopCount;
                this.comment = e.comment;
                this.label = e.label;
                this.dateCreated = e.dateCreated;
            }
        }
    }
}