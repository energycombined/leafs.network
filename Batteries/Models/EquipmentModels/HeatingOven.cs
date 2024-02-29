using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models.EquipmentModels
{
    public class HeatingOven
    {
        public long settingsId { get; set; }
        public long? fkExperimentProcess { get; set; }
        public long? fkBatchProcess { get; set; }
        public int? fkEquipmentModel { get; set; }
        public double? temperature { get; set; }
        public double? heatingTime { get; set; }
        public string atmosphere { get; set; }
        public string comment { get; set; }
        public string label { get; set; }
        public DateTime? dateCreated { get; set; }

    }
}