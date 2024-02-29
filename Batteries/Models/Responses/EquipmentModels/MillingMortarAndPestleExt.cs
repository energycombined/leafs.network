using Batteries.Models.EquipmentModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models.Responses.EquipmentModels
{
    public class MillingMortarAndPestleExt : MillingMortarAndPestle
    {
        public string equipmentModelName { get; set; }
        public MillingMortarAndPestleExt(MillingMortarAndPestle e)
        {
            if (e != null)
            {
                this.settingsId = e.settingsId;
                this.fkExperimentProcess = e.fkExperimentProcess;
                this.fkBatchProcess = e.fkBatchProcess;
                this.fkEquipmentModel = e.fkEquipmentModel;
                this.material = e.material;
                this.comment = e.comment;
                this.label = e.label;
                this.dateCreated = e.dateCreated;
            }
        }
    }
}