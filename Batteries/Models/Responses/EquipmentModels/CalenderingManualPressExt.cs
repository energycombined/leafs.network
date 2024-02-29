using Batteries.Models.EquipmentModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models.Responses.EquipmentModels
{
    public class CalenderingManualPressExt : CalenderingManualPress
    {
        public string equipmentModelName { get; set; }
        public CalenderingManualPressExt(CalenderingManualPress e)
        {
            if (e != null)
            {
                this.settingsId = e.settingsId;
                this.fkExperimentProcess = e.fkExperimentProcess;
                this.fkBatchProcess = e.fkBatchProcess;
                this.fkEquipmentModel = e.fkEquipmentModel;
                this.pressingFoil = e.pressingFoil;
                this.pressingFoilMaterial = e.pressingFoilMaterial;
                this.thickness = e.thickness;
                this.temperature = e.temperature;
                this.pressure = e.pressure;
                this.speed = e.speed;
                this.comment = e.comment;
                this.label = e.label;
                this.dateCreated = e.dateCreated;
            }
        }
    }
}