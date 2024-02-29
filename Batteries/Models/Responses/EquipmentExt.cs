using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models.Responses
{
    public class EquipmentExt : Equipment
    {
        public string processType { get; set; }
        public EquipmentExt(Equipment e = null)
        {
            if (e != null)
            {
                this.equipmentId = e.equipmentId;
                this.equipmentName = e.equipmentName;
                this.equipmentLabel = e.equipmentLabel;
                this.fkProcessType = e.fkProcessType;

            }
        }
    }
}