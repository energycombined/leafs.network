using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models.EquipmentModels
{
    public class CalenderingHeatRollerPress
    {
        public long? settingsId { get; set; }
        public long? fkExperimentProcess { get; set; }
        public long? fkBatchProcess { get; set; }
        public int? fkEquipmentModel { get; set; }
        public Boolean? pressingFoil { get; set; }
        public string pressingFoilMaterial { get; set; }
        public double? thickness { get; set; }
        public double? temperature { get; set; }
        public double? pressure { get; set; }
        public double? speed { get; set; }
        public string comment { get; set; }
        public string label { get; set; }
        public DateTime? dateCreated { get; set; }        
    }
}