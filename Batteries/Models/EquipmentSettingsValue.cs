using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models
{
    public class EquipmentSettingsValue
    {
        public long? equipmentAttributeValueId { get; set; }
        public long? equipmentAttributeTypeId { get; set; }
        public int? fkAttribute { get; set; }
        public int? order { get; set; }
        public string attributeName { get; set; }
        public string value { get; set; }
        public List<EquipmentSettingsValue> children { get; set; } = new List<EquipmentSettingsValue>();
        public int? fkDbType { get; set; }
        public string type { get; set; }
        public string attMeasurementUnit { get; set; }
        public int? fkParentAttribute { get; set; }
        public bool? isParent { get; set; }

    }
}