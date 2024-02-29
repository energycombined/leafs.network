using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models.EquipmentModels
{
    public class MillingMortarAndPestle
    {
        public long settingsId { get; set; }
        public long? fkExperimentProcess { get; set; }
        public long? fkBatchProcess { get; set; }
        public int? fkEquipmentModel { get; set; }
        public string material { get; set; }
        public string comment { get; set; }
        public string label { get; set; }
        public DateTime? dateCreated { get; set; }

    }
}