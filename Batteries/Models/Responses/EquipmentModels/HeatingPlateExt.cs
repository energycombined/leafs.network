using Batteries.Models.EquipmentModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models.Responses.EquipmentModels
{
    public class HeatingPlateExt : HeatingPlate
    {
        public string equipmentModelName { get; set; }
        public HeatingPlateExt(HeatingPlate e)
        {
            if (e != null)
            {
                this.settingsId = e.settingsId;
                this.fkExperimentProcess = e.fkExperimentProcess;
                this.fkBatchProcess = e.fkBatchProcess;
                this.fkEquipmentModel = e.fkEquipmentModel;
                this.temperature = e.temperature;
                this.heatingTime = e.heatingTime;
                this.stirring = e.stirring;
                this.stirringSpeed = e.stirringSpeed;
                this.stirBarSize = e.stirBarSize;
                this.atmosphere = e.atmosphere;
                this.flow = e.flow;
                this.flowRate = e.flowRate;
                this.comment = e.comment;
                this.label = e.label;
                this.dateCreated = e.dateCreated;
            }
        }
    }
}