using Batteries.Models.EquipmentModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models.Responses.EquipmentModels
{
    public class PressingManualExt : PressingManual
    {
        public string equipmentModelName { get; set; }
        public PressingManualExt(PressingManual e)
        {
            if (e != null)
            {
                this.settingsId = e.settingsId;
                this.fkExperimentProcess = e.fkExperimentProcess;
                this.fkBatchProcess = e.fkBatchProcess;
                this.fkEquipmentModel = e.fkEquipmentModel;
                this.pressingBlocks = e.pressingBlocks;
                this.substrateMaterial = e.substrateMaterial;
                this.pressure = e.pressure;
                this.pressingTime = e.pressingTime;
                this.temperature = e.temperature;
                this.comment = e.comment;
                this.label = e.label;
                this.dateCreated = e.dateCreated;
            }
        }
    }
}