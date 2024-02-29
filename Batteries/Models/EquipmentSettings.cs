using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models
{
    public class EquipmentSettings
    {
        public int? equipmentId { get; set; }
        public string equipmentName { get; set; }
        public int? fkExperimentProcess { get; set; }
        public int? fkBatchProcess { get; set; }
        public int? fkSequenceContent { get; set; } 
        public DateTime? dateCreated { get; set; }
        public string label { get; set; }
        public int fkProcessType { get; set; }
        public string processType { get; set; }
        public string subcategory { get; set; }
        public int? equipmentModelId { get; set; }
        public string equipmentModelName { get; set; }
        public string modelBrand { get; set; }
        public List<EquipmentSettingsValue> equipmentSettingsValues { get; set; } = new List<EquipmentSettingsValue>();
        public long? equipmentAttributeValueId { get; set; }
        public long? equipmentAttributeTypeId { get; set; }
        public int? fkAttribute { get; set; }
        public int? order { get; set; }
        public string attributeName { get; set; }
        public string value { get; set; }
        public int? fkDbType { get; set; }
        public string type { get; set; }
       
    }
}