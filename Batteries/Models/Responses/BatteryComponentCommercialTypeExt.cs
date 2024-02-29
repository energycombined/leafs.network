using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models.Responses
{
    public class BatteryComponentCommercialTypeExt : BatteryComponentCommercialType
    {
        public BatteryComponentCommercialTypeExt(BatteryComponentCommercialType e = null)
        {
            if (e != null)
            {
                this.batteryComponentCommercialTypeId = e.batteryComponentCommercialTypeId;
                this.fkBatteryComponentType = e.fkBatteryComponentType;
                this.batteryComponentCommercialType = e.batteryComponentCommercialType;
                this.model = e.model;
                this.fkResearchGroup = e.fkResearchGroup;
            }
        }
    }
}