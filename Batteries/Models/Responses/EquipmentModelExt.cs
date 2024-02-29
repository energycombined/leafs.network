using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models.Responses
{
    public class EquipmentModelExt : EquipmentModel
    {
        public string equipmentName { get; set; }
        public EquipmentModelExt(EquipmentModel e)
        {
            if (e != null)
            {
                this.equipmentModelId = e.equipmentModelId;
                this.fkEquipment = e.fkEquipment;
                this.equipmentModelName = e.equipmentModelName;
                this.modelBrand = e.modelBrand;
            }
        }
    }
}