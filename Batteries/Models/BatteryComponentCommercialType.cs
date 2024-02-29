using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models
{
    public class BatteryComponentCommercialType
    {
        public long batteryComponentCommercialTypeId { get; set; }
        public int? fkBatteryComponentType { get; set; }
        public string batteryComponentCommercialType { get; set; }
        public string model { get; set; }
        public int? fkResearchGroup { get; set; }
    }
}