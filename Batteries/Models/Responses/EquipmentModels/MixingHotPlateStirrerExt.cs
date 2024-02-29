using Batteries.Models.EquipmentModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models.Responses.EquipmentModels
{
    public class MixingHotPlateStirrerExt : MixingHotPlateStirrer
    {
        public string equipmentModelName { get; set; }
        public MixingHotPlateStirrerExt(MixingHotPlateStirrer e)
        {
            if (e != null)
            {
                this.settingsId = e.settingsId;
                this.fkExperimentProcess = e.fkExperimentProcess;
                this.fkBatchProcess = e.fkBatchProcess;
                this.fkEquipmentModel = e.fkEquipmentModel;
                this.rotationSpeed = e.rotationSpeed;
                this.rotationTime = e.rotationTime;
                this.restTime = e.restTime;
                this.temperature = e.temperature;
                this.comment = e.comment;
                this.label = e.label;
                this.dateCreated = e.dateCreated;
            }
        }
    }
}